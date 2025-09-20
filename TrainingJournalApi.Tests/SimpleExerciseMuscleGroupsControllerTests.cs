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

namespace TrainingJournalApi.Tests
{
    public class SimpleExerciseMuscleGroupsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SimpleExerciseMuscleGroupsControllerTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestDbForExerciseMuscleGroups");
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
        public async Task GetExerciseMuscleGroups_WithoutAuthentication_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/ExerciseMuscleGroups");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetExerciseMuscleGroupById_WithoutAuthentication_ReturnsMethodNotAllowed()
        {
            // Act
            var response = await _client.GetAsync("/api/ExerciseMuscleGroups/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task CreateExerciseMuscleGroup_WithoutAuthentication_ReturnsNotFound()
        {
            // Arrange
            var createDto = new CreateExerciseMuscleGroupDto
            {
                MuscleGroup = MuscleGroup.Chest,
                Role = MuscleGroupRole.Primary
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/ExerciseMuscleGroups", createDto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateExerciseMuscleGroup_WithoutAuthentication_ReturnsMethodNotAllowed()
        {
            // Arrange
            var updateDto = new UpdateExerciseMuscleGroupDto
            {
                MuscleGroup = MuscleGroup.Back,
                Role = MuscleGroupRole.Secondary
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/ExerciseMuscleGroups/1", updateDto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task DeleteExerciseMuscleGroup_WithoutAuthentication_ReturnsMethodNotAllowed()
        {
            // Act
            var response = await _client.DeleteAsync("/api/ExerciseMuscleGroups/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
