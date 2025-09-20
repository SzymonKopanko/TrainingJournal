using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainingJournalApi.Data;
using Microsoft.AspNetCore.Identity;
using TrainingJournalApi.Models;
using TrainingJournalApi.DTOs;
using Xunit;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using System.Net.Http.Headers;

namespace TrainingJournalApi.Tests
{
    public class AuthenticatedExerciseSetsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private ApplicationUser _testUser;

        public AuthenticatedExerciseSetsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the app's ApplicationDbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<TrainingJournalApiContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add InMemory database
                    services.AddDbContext<TrainingJournalApiContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDbForAuthenticatedExerciseSets");
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
            
            // Create client
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

        private async Task SetupTestDataAsync(string userId)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrainingJournalApiContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if exercise already exists for this user
            var existingExercise = await context.Exercises.FirstOrDefaultAsync(e => e.UserId == userId);
            if (existingExercise == null)
            {
                var exercise = new Exercise
                {
                    Name = "Test Exercise",
                    Description = "Test Description",
                    BodyWeightPercentage = 0.5,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync(); // Save to get the ID
            }

            // Check if exercise entry already exists for this user
            var existingEntry = await context.ExerciseEntries.FirstOrDefaultAsync(e => e.UserId == userId);
            if (existingEntry == null)
            {
                var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.UserId == userId);
                if (exercise == null)
                {
                    throw new InvalidOperationException($"Exercise not found for user {userId}. Make sure SetupTestDataAsync creates Exercise first.");
                }
                
                var exerciseEntry = new ExerciseEntry
                {
                    ExerciseId = exercise.Id,
                    UserId = userId,
                    Notes = "Test entry",
                    CreatedAt = DateTime.UtcNow
                };
                context.ExerciseEntries.Add(exerciseEntry);
                await context.SaveChangesAsync(); // Save to get the ID
            }

            // Check if user weight already exists for this user
            var existingWeight = await context.UserWeights.FirstOrDefaultAsync(w => w.UserId == userId);
            if (existingWeight == null)
            {
                var userWeight = new UserWeight
                {
                    UserId = userId,
                    Weight = 80.0,
                    WeightedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                context.UserWeights.Add(userWeight);
            }

            await context.SaveChangesAsync();
        }

        private (HttpClient client, string userId) GetAuthenticatedClient()
        {
            Console.WriteLine("=== Creating authenticated client ===");
            
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
            
            Console.WriteLine($"Created authenticated client with userId: {userId}");
            return (client, userId);
        }

        [Fact]
        public async Task GetExerciseSets_WithAuthentication_ReturnsOk()
        {
            // Arrange
            Console.WriteLine("=== Starting GetExerciseSets_WithAuthentication_ReturnsOk test ===");
            var (authenticatedClient, userId) = GetAuthenticatedClient();
            Console.WriteLine($"Created authenticated client: {authenticatedClient != null}");

            // Act
            Console.WriteLine("Making GET request to /api/ExerciseSets/test");
            var response = await authenticatedClient.GetAsync("/api/ExerciseSets/test");
            Console.WriteLine($"Response status: {response.StatusCode}");
            Console.WriteLine($"Response headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");
            
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateExerciseSet_WithValidData_ReturnsCreated()
        {
            // Arrange
            Console.WriteLine("=== Starting CreateExerciseSet_WithValidData_ReturnsCreated test ===");
            var (authenticatedClient, userId) = GetAuthenticatedClient();
            
            // Create Exercise using API
            var createExerciseDto = new CreateExerciseDto
            {
                Name = "Test Exercise",
                Description = "Test Description",
                BodyWeightPercentage = 0.5
            };
            
            Console.WriteLine("Creating Exercise via API");
            var exerciseResponse = await authenticatedClient.PostAsJsonAsync("/api/Exercises", createExerciseDto);
            Console.WriteLine($"Exercise creation response status: {exerciseResponse.StatusCode}");
            var exerciseDto = await exerciseResponse.Content.ReadFromJsonAsync<ExerciseDto>();
            Console.WriteLine($"Created Exercise with ID: {exerciseDto?.Id}");
            
            // Create ExerciseEntry using API
            var createExerciseEntryDto = new CreateExerciseEntryDto
            {
                ExerciseId = exerciseDto!.Id,
                Notes = "Test entry"
            };
            
            Console.WriteLine("Creating ExerciseEntry via API");
            var exerciseEntryResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseEntries", createExerciseEntryDto);
            Console.WriteLine($"ExerciseEntry creation response status: {exerciseEntryResponse.StatusCode}");
            
            if (exerciseEntryResponse.StatusCode != HttpStatusCode.Created)
            {
                var errorContent = await exerciseEntryResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"ExerciseEntry creation failed: {errorContent}");
                throw new InvalidOperationException($"ExerciseEntry creation failed with status {exerciseEntryResponse.StatusCode}: {errorContent}");
            }
            
            var exerciseEntryDto = await exerciseEntryResponse.Content.ReadFromJsonAsync<ExerciseEntryDto>();
            Console.WriteLine($"Created ExerciseEntry with ID: {exerciseEntryDto?.Id}");
            
            // Create UserWeight using API
            var createUserWeightDto = new CreateUserWeightDto
            {
                Weight = 80.0,
                WeightedAt = DateTime.UtcNow
            };
            
            Console.WriteLine("Creating UserWeight via API");
            var userWeightResponse = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", createUserWeightDto);
            Console.WriteLine($"UserWeight creation response status: {userWeightResponse.StatusCode}");
            
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = exerciseEntryDto!.Id,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };

            Console.WriteLine("Making POST request to /api/ExerciseSets");
            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            Console.WriteLine($"Response status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("\"id\":1", content);
        }

        [Fact]
        public async Task CreateExerciseSet_WithInvalidExerciseEntryId_ReturnsBadRequest()
        {
            // Arrange
            Console.WriteLine("=== Starting CreateExerciseSet_WithInvalidExerciseEntryId_ReturnsBadRequest test ===");
            var (authenticatedClient, userId) = GetAuthenticatedClient();
            await SetupTestDataAsync(userId); // Ensure test data is available
            
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = 999, // Non-existent exercise entry
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };

            Console.WriteLine("Making POST request to /api/ExerciseSets with invalid data");
            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            Console.WriteLine($"Response status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateExerciseSet_WithValidData_ReturnsNoContent()
        {
            // Arrange
            Console.WriteLine("=== Starting UpdateExerciseSet_WithValidData_ReturnsNoContent test ===");
            var (authenticatedClient, userId) = GetAuthenticatedClient();
            await SetupTestDataAsync(userId); // Ensure test data is available
            
            // First create an exercise set
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrainingJournalApiContext>();
            var exerciseEntry = await context.ExerciseEntries.FirstOrDefaultAsync(e => e.UserId == userId);
            
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = exerciseEntry!.Id,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };
            Console.WriteLine("Creating initial ExerciseSet for update test");
            var createResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            var createdSet = await createResponse.Content.ReadFromJsonAsync<ExerciseSetDto>();
            Console.WriteLine($"Initial ExerciseSet created with ID: {createdSet?.Id}, Status: {createResponse.StatusCode}");

            var updateDto = new UpdateExerciseSetDto
            {
                Order = 2,
                Reps = 12,
                Weight = 55.0,
                RIR = 1
            };

            Console.WriteLine($"Making PUT request to /api/ExerciseSets/{createdSet!.Id}");
            // Act
            var response = await authenticatedClient.PutAsJsonAsync($"/api/ExerciseSets/{createdSet!.Id}", updateDto);
            Console.WriteLine($"Response status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteExerciseSet_WithValidId_ReturnsNoContent()
        {
            // Arrange
            Console.WriteLine("=== Starting DeleteExerciseSet_WithValidId_ReturnsNoContent test ===");
            var (authenticatedClient, userId) = GetAuthenticatedClient();
            await SetupTestDataAsync(userId); // Ensure test data is available
            
            // First create an exercise set
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrainingJournalApiContext>();
            var exerciseEntry = await context.ExerciseEntries.FirstOrDefaultAsync(e => e.UserId == userId);
            
            var createDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = exerciseEntry!.Id,
                Order = 1,
                Reps = 10,
                Weight = 50.0,
                RIR = 2
            };
            Console.WriteLine("Creating initial ExerciseSet for delete test");
            var createResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", createDto);
            var createdSet = await createResponse.Content.ReadFromJsonAsync<ExerciseSetDto>();
            Console.WriteLine($"Initial ExerciseSet created with ID: {createdSet?.Id}, Status: {createResponse.StatusCode}");

            Console.WriteLine($"Making DELETE request to /api/ExerciseSets/{createdSet!.Id}");
            // Act
            var response = await authenticatedClient.DeleteAsync($"/api/ExerciseSets/{createdSet!.Id}");
            Console.WriteLine($"Response status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
