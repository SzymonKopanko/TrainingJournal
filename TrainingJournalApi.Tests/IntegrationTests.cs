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
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase($"TestDbForIntegration_{Guid.NewGuid()}");
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

        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            // Register user
            var registerDto = new RegisterDto
            {
                Email = "integration@example.com",
                Password = "IntegrationTest123!",
                FirstName = "Integration",
                LastName = "Test",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Login user
            var loginDto = new LoginDto
            {
                Email = "integration@example.com",
                Password = "IntegrationTest123!"
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
        public async Task CompleteWorkoutScenario_CreateExerciseAndLogWorkout_ReturnsSuccess()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();

            // Step 1: Create an exercise
            var exerciseDto = new CreateExerciseDto
            {
                Name = "Bench Press",
                Description = "Chest exercise",
                BodyWeightPercentage = 0.0,
                ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>
                {
                    new CreateExerciseMuscleGroupDto
                    {
                        MuscleGroup = MuscleGroup.Chest,
                        Role = MuscleGroupRole.Primary
                    }
                }
            };

            var exerciseResponse = await authenticatedClient.PostAsJsonAsync("/api/Exercises", exerciseDto);
            Assert.Equal(HttpStatusCode.Created, exerciseResponse.StatusCode);
            var exercise = await exerciseResponse.Content.ReadFromJsonAsync<ExerciseDto>();

            // Step 2: Log user weight
            var weightDto = new CreateUserWeightDto
            {
                Weight = 80.0,
                WeightedAt = DateTime.UtcNow
            };

            var weightResponse = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", weightDto);
            Assert.Equal(HttpStatusCode.Created, weightResponse.StatusCode);

            // Step 3: Create exercise entry (workout session)
            var entryDto = new CreateExerciseEntryDto
            {
                ExerciseId = exercise!.Id,
                Notes = "Great workout today!"
            };

            var entryResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseEntries", entryDto);
            Assert.Equal(HttpStatusCode.Created, entryResponse.StatusCode);
            var entry = await entryResponse.Content.ReadFromJsonAsync<ExerciseEntryDto>();

            // Step 4: Log exercise sets
            var sets = new[]
            {
                new CreateExerciseSetDto
                {
                    ExerciseEntryId = entry!.Id,
                    Order = 1,
                    Reps = 10,
                    Weight = 60.0,
                    RIR = 2
                },
                new CreateExerciseSetDto
                {
                    ExerciseEntryId = entry.Id,
                    Order = 2,
                    Reps = 8,
                    Weight = 65.0,
                    RIR = 1
                },
                new CreateExerciseSetDto
                {
                    ExerciseEntryId = entry.Id,
                    Order = 3,
                    Reps = 6,
                    Weight = 70.0,
                    RIR = 0
                }
            };

            foreach (var setDto in sets)
            {
                var setResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", setDto);
                Assert.Equal(HttpStatusCode.Created, setResponse.StatusCode);
            }

            // Step 5: Verify the workout was logged correctly
            var setsResponse = await authenticatedClient.GetAsync("/api/ExerciseSets");
            Assert.Equal(HttpStatusCode.OK, setsResponse.StatusCode);
            
            var loggedSets = await setsResponse.Content.ReadFromJsonAsync<List<ExerciseSetDto>>();
            Assert.Equal(3, loggedSets!.Count);
            Assert.All(loggedSets, set => Assert.True(set.OneRepMax > 0)); // Verify 1RM calculations
        }

        [Fact]
        public async Task CreateTrainingPlanScenario_CreateTrainingWithExercises_ReturnsSuccess()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();

            // Step 1: Create exercises
            var exercises = new[]
            {
                new CreateExerciseDto
                {
                    Name = "Squat",
                    Description = "Leg exercise",
                    BodyWeightPercentage = 0.0,
                    ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>
                    {
                        new CreateExerciseMuscleGroupDto
                        {
                            MuscleGroup = MuscleGroup.Quads,
                            Role = MuscleGroupRole.Primary
                        }
                    }
                },
                new CreateExerciseDto
                {
                    Name = "Deadlift",
                    Description = "Back exercise",
                    BodyWeightPercentage = 0.0,
                    ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>
                    {
                        new CreateExerciseMuscleGroupDto
                        {
                            MuscleGroup = MuscleGroup.Back,
                            Role = MuscleGroupRole.Primary
                        }
                    }
                }
            };

            var createdExercises = new List<ExerciseDto>();
            foreach (var exerciseDto in exercises)
            {
                var response = await authenticatedClient.PostAsJsonAsync("/api/Exercises", exerciseDto);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                var exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
                createdExercises.Add(exercise!);
            }

            // Step 2: Create training plan
            var trainingDto = new CreateTrainingDto
            {
                Name = "Leg Day",
                Description = "Lower body workout"
            };

            var trainingResponse = await authenticatedClient.PostAsJsonAsync("/api/Trainings", trainingDto);
            Assert.Equal(HttpStatusCode.Created, trainingResponse.StatusCode);
            var training = await trainingResponse.Content.ReadFromJsonAsync<TrainingDto>();

            // Step 3: Add exercises to training
            var trainingExercises = new[]
            {
                new CreateTrainingExerciseDto
                {
                    ExerciseId = createdExercises[0].Id,
                    Order = 1,
                    Notes = "Start with squats"
                },
                new CreateTrainingExerciseDto
                {
                    ExerciseId = createdExercises[1].Id,
                    Order = 2,
                    Notes = "Finish with deadlifts"
                }
            };

            foreach (var trainingExerciseDto in trainingExercises)
            {
                var response = await authenticatedClient.PostAsJsonAsync("/api/TrainingExercises", trainingExerciseDto);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }

            // Step 4: Verify training plan
            var trainingExercisesResponse = await authenticatedClient.GetAsync("/api/TrainingExercises");
            Assert.Equal(HttpStatusCode.OK, trainingExercisesResponse.StatusCode);
            
            var loggedTrainingExercises = await trainingExercisesResponse.Content.ReadFromJsonAsync<List<TrainingExerciseDto>>();
            Assert.Equal(2, loggedTrainingExercises!.Count);
        }

        [Fact]
        public async Task UserProgressTrackingScenario_TrackWeightAndWorkouts_ReturnsSuccess()
        {
            // Arrange
            var authenticatedClient = await GetAuthenticatedClientAsync();

            // Step 1: Log initial weight
            var initialWeightDto = new CreateUserWeightDto
            {
                Weight = 80.0,
                WeightedAt = DateTime.UtcNow.AddDays(-30)
            };

            var weightResponse = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", initialWeightDto);
            Assert.Equal(HttpStatusCode.Created, weightResponse.StatusCode);

            // Step 2: Create exercise and log workout
            var exerciseDto = new CreateExerciseDto
            {
                Name = "Push-ups",
                Description = "Bodyweight exercise",
                BodyWeightPercentage = 0.6,
                ExerciseMuscleGroups = new List<CreateExerciseMuscleGroupDto>
                {
                    new CreateExerciseMuscleGroupDto
                    {
                        MuscleGroup = MuscleGroup.Chest,
                        Role = MuscleGroupRole.Primary
                    }
                }
            };

            var exerciseResponse = await authenticatedClient.PostAsJsonAsync("/api/Exercises", exerciseDto);
            var exercise = await exerciseResponse.Content.ReadFromJsonAsync<ExerciseDto>();

            var entryDto = new CreateExerciseEntryDto
            {
                ExerciseId = exercise!.Id,
                Notes = "First workout"
            };

            var entryResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseEntries", entryDto);
            var entry = await entryResponse.Content.ReadFromJsonAsync<ExerciseEntryDto>();

            var setDto = new CreateExerciseSetDto
            {
                ExerciseEntryId = entry!.Id,
                Order = 1,
                Reps = 15,
                Weight = 0.0, // Bodyweight exercise
                RIR = 2
            };

            var setResponse = await authenticatedClient.PostAsJsonAsync("/api/ExerciseSets", setDto);
            Assert.Equal(HttpStatusCode.Created, setResponse.StatusCode);

            // Step 3: Log current weight (after 30 days)
            var currentWeightDto = new CreateUserWeightDto
            {
                Weight = 82.0, // Gained 2kg
                WeightedAt = DateTime.UtcNow
            };

            var currentWeightResponse = await authenticatedClient.PostAsJsonAsync("/api/UserWeights", currentWeightDto);
            Assert.Equal(HttpStatusCode.Created, currentWeightResponse.StatusCode);

            // Step 4: Verify progress tracking
            var weightsResponse = await authenticatedClient.GetAsync("/api/UserWeights");
            Assert.Equal(HttpStatusCode.OK, weightsResponse.StatusCode);
            
            var weights = await weightsResponse.Content.ReadFromJsonAsync<List<UserWeightDto>>();
            Assert.Equal(2, weights!.Count);
            Assert.Equal(80.0, weights[0].Weight);
            Assert.Equal(82.0, weights[1].Weight);
        }
    }
}
