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
using Microsoft.AspNetCore.Authentication;

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
                        options.UseInMemoryDatabase("TestDbForValidation");
                    });

                    // Configure Test Authentication as default
                    services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = "TestScheme";
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });

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

        private HttpClient GetAuthenticatedClient()
        {
            // Create a new HttpClient with Test Authentication
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                HandleCookies = true
            };
            var client = _factory.CreateClient(clientOptions);
            client.DefaultRequestHeaders.Clear(); // Clear any existing headers
            
            // Generate a unique userId for this test
            var userId = $"test-user-{Guid.NewGuid()}";
            
            // Add X-Test-User-Id header for TestAuthHandler
            client.DefaultRequestHeaders.Add("X-Test-User-Id", userId);
            
            return client;
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
            var authenticatedClient = GetAuthenticatedClient();
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
            var authenticatedClient = GetAuthenticatedClient();
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
            var authenticatedClient = GetAuthenticatedClient();
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
            var authenticatedClient = GetAuthenticatedClient();
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
        public async Task CreateUserWeight_WithNegativeWeight_ReturnsMethodNotAllowed()
        {
            // Arrange
            var authenticatedClient = GetAuthenticatedClient();
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
        public async Task CreateUserWeight_WithFutureDate_ReturnsMethodNotAllowed()
        {
            // Arrange
            var authenticatedClient = GetAuthenticatedClient();
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
