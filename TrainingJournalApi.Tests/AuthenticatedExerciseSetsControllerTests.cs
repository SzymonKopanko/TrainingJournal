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
    public class AuthenticatedExerciseSetsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationUser _testUser;

        public AuthenticatedExerciseSetsControllerTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase($"TestDbForAuthenticatedExerciseSets_{Guid.NewGuid()}");
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
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180.0
            };
        }

        private async Task SetupTestDataAsync()
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

            // Check if user weight already exists
            var existingWeight = await context.UserWeights.FindAsync(1);
            if (existingWeight == null)
            {
                var userWeight = new UserWeight
                {
                    Id = 1,
                    UserId = _testUser.Id,
                    Weight = 80.0,
                    WeightedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                context.UserWeights.Add(userWeight);
            }

            await context.SaveChangesAsync();
        }

        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            await SetupTestDataAsync();
            
            // Login user
            var loginDto = new LoginDto
            {
                Email = _testUser.Email,
                Password = "TestPassword123!"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            if (loginResponse.IsSuccessStatusCode)
            {
                // Get cookies from response
                var cookieHeader = loginResponse.Headers.GetValues("Set-Cookie").FirstOrDefault();
                if (!string.IsNullOrEmpty(cookieHeader))
                {
                    var authenticatedClient = _factory.CreateClient();
                    authenticatedClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
                    return authenticatedClient;
                }
            }

            return _client; // Fallback to unauthenticated client
        }

        [Fact]
        public async Task GetExerciseSets_WithAuthentication_ReturnsOk()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();

            // Act
            var response = await authenticatedClient.GetAsync("/api/ExerciseSets");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateExerciseSet_WithValidData_ReturnsCreated()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 1,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("ExerciseSet", content);
        }

        [Fact]
        public async Task CreateExerciseSet_WithInvalidExerciseEntryId_ReturnsBadRequest()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 999, // Non-existent exercise entry
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateExerciseSet_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            
            // First create an exercise set
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 1,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };
            var createResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            var createdSet = await createResponse.Content.ReadFromJsonAsync<ExerciseSetDto>();

            var updateDto = new UpdateExerciseSetDto
            {
                Order = 2,
                Reps = 12,
                Weight = 55.0,
                RIR = 1
            };

            // Act
            var response = await authenticatedClient.PutAsJsonAsync($"/api/ExerciseSets/{createdSet!.Id}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteExerciseSet_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();
            
            // First create an exercise set
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 1,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };
            var createResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            var createdSet = await createResponse.Content.ReadFromJsonAsync<ExerciseSetDto>();

            // Act
            var response = await authenticatedClient.DeleteAsync($"/api/ExerciseSets/{createdSet!.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
