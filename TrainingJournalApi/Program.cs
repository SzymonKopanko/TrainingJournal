using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Data;
using TrainingJournalApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TrainingJournalApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracja ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Konfiguracja hasła
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    
    // Konfiguracja użytkownika
    options.User.RequireUniqueEmail = true;
    
    // Konfiguracja logowania
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<TrainingJournalApiContext>()
.AddDefaultTokenProviders();

// Konfiguracja cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/api/account/login";
    options.LogoutPath = "/api/account/logout";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Dodanie middleware dla uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Make Program class available for testing
public partial class Program { }