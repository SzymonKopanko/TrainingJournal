using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainingJournalApi.Data;
using Microsoft.AspNetCore.Identity;
using TrainingJournalApi.Models;
using System.Net.Http.Json;
using System.Text.Json;
using TrainingJournalApi.DTOs;
using System.Net;

namespace TrainingJournalApi.Tests
{
    public class ValidationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationUser _testUser;

        public ValidationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<TrainingJournalApiContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add InMemory database
                    services.AddDbContext<TrainingJournalApiContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDbForValidation_{Guid.NewGuid()}");
                    });

                    // Remove existing UserStore
                    var userStoreDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IUserStore<ApplicationUser>));
                    if (userStoreDescriptor != null)
                    {
                        services.Remove(userStoreDescriptor);
                    }

                    // Add InMemory UserStore
                    services.AddIdentityCore<ApplicationUser>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 1;
                    })
                    .AddEntityFrameworkStores<TrainingJournalApiContext>()
                    .AddDefaultTokenProviders();

                    // Disable logging during tests
                    services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
                });
            });
            
            _client = _factory.CreateClient();
            
            // Create test user
            _testUser = new ApplicationUser
            {
                Id = $"test-user-id-{Guid.NewGuid()}",
                UserName = $"testuser{Guid.NewGuid()}@example.com",
                Email = $"testuser{Guid.NewGuid()}@example.com",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180.0
            };
        }

        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrainingJournalApiContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if user already exists
            var existingUser = await userManager.FindByIdAsync(_testUser.Id);
            if (existingUser == null)
            {
                await userManager.CreateAsync(_testUser, "TestPassword123!");
            }

            // Check if exercise already exists
            var existingExercise = await context.Exercises.FindAsync(1);
            if (existingExercise == null)
            {
                var exercise = new Exercise
                {
                    Id = 1,
                    Name = "Test Exercise",
                    Description = "Test Description",
                    BodyWeightPercentage = 0.5,
                    UserId = _testUser.Id,
                    CreatedAt = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
            }

            // Check if exercise entry already exists
            var existingEntry = await context.ExerciseEntries.FindAsync(1);
            if (existingEntry == null)
            {
                var exerciseEntry = new ExerciseEntry
                {
                    Id = 1,
                    ExerciseId = 1,
                    UserId = _testUser.Id,
                    Notes = "Test entry",
                    CreatedAt = DateTime.UtcNow
                };
                context.ExerciseEntries.Add(exerciseEntry);
            }

            await context.SaveChangesAsync();

            // Login user
            var loginDto = new LoginDto
            {
                Email = _testUser.Email,
                Password = "TestPassword123!"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            if (loginResponse.IsSuccessStatusCode)
            {
                var cookies = loginResponse.Headers.GetValues("Set-Cookie");
                var authenticatedClient = _factory.CreateClient();
                foreach (var cookie in cookies)
                {
                    authenticatedClient.DefaultRequestHeaders.Add("Cookie", cookie);
                }
                return authenticatedClient;
            }

            return _client;
        }

        [Fact]
        public async Task Register_WithInvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "invalid-email", // Invalid email format
                Password = "ValidPassword123!",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithShortPassword_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = $"test{Guid.NewGuid()}@example.com",
                Password = "123", // Too short password
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithFutureDateOfBirth_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = $"test{Guid.NewGuid()}@example.com",
                Password = "ValidPassword123!",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = DateTime.UtcNow.AddDays(1), // Future date
                Height = 180
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithNegativeHeight_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = $"test{Guid.NewGuid()}@example.com",
                Password = "ValidPassword123!",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = -10 // Negative height
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateExercise_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseDto
            {
                Name = "", // Empty name
                Description = "Test Description",
                BodyWeightPercentage = 0.5,
                ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>()
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/Exercises", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateExercise_WithNegativeBodyWeightPercentage_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseDto
            {
                Name = "Test Exercise",
                Description = "Test Description",
                BodyWeightPercentage = -0.5, // Negative percentage
                ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>()
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/Exercises", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateExerciseSet_WithNegativeReps_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 1,
                Order = 1,
                Reps = -5, // Negative reps
                Weight = 50.0,
                RIR = 2
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateExerciseSet_WithNegativeWeight_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 1,
                Order = 1,
                Reps = 10,
                Weight = -50.0, // Negative weight
                RIR = 2
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWeight_WithNegativeWeight_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateUserWeightDto
            {
                Weight = -80.0, // Negative weight
                WeightedAt = DateTime.UtcNow
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWeight_WithFutureDate_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateUserWeightDto
            {
                Weight = 80.0,
                WeightedAt = DateTime.UtcNow.AddDays(1) // Future date
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
