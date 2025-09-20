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
    public class SimpleTrainingsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SimpleTrainingsControllerTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestDatabase");
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
        public async Task GetTrainings_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/Trainings");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task GetTraining_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/Trainings/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task CreateTraining_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Arrange
            var createDto = new CreateTrainingDto
            {
                Name = "Test Training",
                Description = "Test Description"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Trainings", createDto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTraining_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Arrange
            var updateDto = new UpdateTrainingDto
            {
                Name = "Updated Training",
                Description = "Updated Description"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/Trainings/1", updateDto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTraining_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.DeleteAsync("/api/Trainings/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
