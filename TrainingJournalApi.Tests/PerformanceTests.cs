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
using System.Diagnostics;

namespace TrainingJournalApi.Tests
{
    public class PerformanceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PerformanceTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase($"TestDbForPerformance_{Guid.NewGuid()}");
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
        }

        [Fact]
        public async Task GetExercises_ResponseTime_ShouldBeUnder100ms()
        {
            // Arrange
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.GetAsync("/api/Exercises");
            stopwatch.Stop();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 100, 
                $"Response time was {stopwatch.ElapsedMilliseconds}ms, expected under 100ms");
        }

        [Fact]
        public async Task GetEnums_ResponseTime_ShouldBeUnder50ms()
        {
            // Arrange
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.GetAsync("/api/Enums");
            stopwatch.Stop();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 50, 
                $"Response time was {stopwatch.ElapsedMilliseconds}ms, expected under 50ms");
        }

        [Fact]
        public async Task RegisterUser_ResponseTime_ShouldBeUnder200ms()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = $"perf{Guid.NewGuid()}@example.com",
                Password = "PerformanceTest123!",
                FirstName = "Performance",
                LastName = "Test",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);
            stopwatch.Stop();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < 200, 
                $"Response time was {stopwatch.ElapsedMilliseconds}ms, expected under 200ms");
        }

        [Fact]
        public async Task MultipleConcurrentRequests_ShouldHandleGracefully()
        {
            // Arrange
            var tasks = new List<Task<HttpResponseMessage>>();
            var numberOfRequests = 10;

            // Act
            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks.Add(_client.GetAsync("/api/Exercises"));
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(numberOfRequests, responses.Length);
            Assert.All(responses, response => 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode));
        }

        [Fact]
        public async Task LargeDataPayload_ShouldBeHandledEfficiently()
        {
            // Arrange
            var largeExerciseDto = new CreateExerciseDto
            {
                Name = new string('A', 100), // 100 character name
                Description = new string('B', 500), // 500 character description
                BodyWeightPercentage = 0.5,
                ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>
                {
                    new CreateExerciseMuscleGroupDto { MuscleGroup = MuscleGroup.Chest, Role = MuscleGroupRole.Primary },
                    new CreateExerciseMuscleGroupDto { MuscleGroup = MuscleGroup.Triceps, Role = MuscleGroupRole.Secondary },
                    new CreateExerciseMuscleGroupDto { MuscleGroup = MuscleGroup.FrontDeltoid, Role = MuscleGroupRole.Secondary }
                }
            };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _client.PostAsJsonAsync("/api/Exercises", largeExerciseDto);
            stopwatch.Stop();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // Expected for unauthenticated request
            Assert.True(stopwatch.ElapsedMilliseconds < 300, 
                $"Response time was {stopwatch.ElapsedMilliseconds}ms, expected under 300ms for large payload");
        }

        [Fact]
        public async Task MemoryUsage_ShouldBeReasonable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act - Perform multiple operations
            for (int i = 0; i < 50; i++)
            {
                var response = await _client.GetAsync("/api/Exercises");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;

            // Assert
            Assert.True(memoryIncrease < 10 * 1024 * 1024, // Less than 10MB increase
                $"Memory increase was {memoryIncrease / 1024 / 1024}MB, expected under 10MB");
        }

        [Fact]
        public async Task DatabaseConnection_ShouldBeEfficient()
        {
            // Arrange
            var stopwatch = Stopwatch.StartNew();

            // Act - Multiple database operations
            var tasks = new List<Task<HttpResponseMessage>>();
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(_client.GetAsync("/api/Exercises"));
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            Assert.All(responses, response => 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode));
            
            var averageResponseTime = stopwatch.ElapsedMilliseconds / tasks.Count;
            Assert.True(averageResponseTime < 50, 
                $"Average response time was {averageResponseTime}ms, expected under 50ms");
        }
    }
}
