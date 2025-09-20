using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TrainingJournalApi.Tests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine("TestAuthHandler.HandleAuthenticateAsync called");
            
            // Sprawdź czy jest header X-Test-User-Id
            if (Request.Headers.ContainsKey("X-Test-User-Id"))
            {
                var userId = Request.Headers["X-Test-User-Id"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine($"TestAuthHandler: Found X-Test-User-Id header: {userId}");
                    
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, $"TestUser-{userId}"),
                        new Claim(ClaimTypes.Email, $"testuser-{userId}@example.com")
                    };
                    
                    var identity = new ClaimsIdentity(claims, "TestScheme");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, "TestScheme");
                    
                    Console.WriteLine($"TestAuthHandler: Successfully authenticated user {userId}");
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }
            
            Console.WriteLine("TestAuthHandler: No X-Test-User-Id header found, returning NoResult");
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
