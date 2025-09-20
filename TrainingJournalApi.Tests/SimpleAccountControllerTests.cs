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
    public class SimpleAccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SimpleAccountControllerTests(WebApplicationFactory<Program> factory)
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

                // builder.UseEnvironment("Testing");
            });
            
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "newuser@example.com",
                Password = "Test123!",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Użytkownik został zarejestrowany pomyślnie", content);
        }

        [Fact]
        public async Task Register_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "", // Invalid email
                Password = "", // Invalid password
                FirstName = "",
                LastName = "",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Account/register", registerDto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Logout_ReturnsOk()
        {
            // Arrange & Act
            var response = await _client.PostAsync("/api/Account/logout", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Wylogowano pomyślnie", content);
        }
    }
}
