using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Data;
using TrainingJournalApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TrainingJournal API",
        Version = "v1",
        Description = "ASP.NET Core 8.0 API for training journal management with Entity Framework and Identity",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "TrainingJournal API",
            Email = "skopanko320@gmail.com"
        }
    });
    
    // Dodaj komentarze XML do dokumentacji
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});


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
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Działa z HTTP i HTTPS
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainingJournal API v1");
        c.RoutePrefix = "swagger"; // Swagger UI będzie dostępny pod /swagger
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
    });
}

app.UseHttpsRedirection();

// Dodanie middleware dla uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Make Program class available for testing
public partial class Program { }