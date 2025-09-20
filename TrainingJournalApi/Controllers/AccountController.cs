using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainingJournalApi.Models;
using TrainingJournalApi.DTOs;
using TrainingJournalApi.Data;

namespace TrainingJournalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TrainingJournalApiContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TrainingJournalApiContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Height = model.Height,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Użytkownik został zarejestrowany pomyślnie" });
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(new { message = "Logowanie pomyślne" });
            }

            return Unauthorized(new { message = "Nieprawidłowy email lub hasło" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Wylogowano pomyślnie" });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Height = user.Height,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };

            return Ok(userDto);
        }

        [HttpPost("admin/authorize-user")]
        public async Task<IActionResult> AuthorizeUserForTesting([FromBody] AuthorizeUserDto model)
        {
            // Tylko dla testów - w produkcji to powinno być zabezpieczone
            if (!HttpContext.Request.Headers.ContainsKey("X-Test-Admin-Key") || 
                HttpContext.Request.Headers["X-Test-Admin-Key"] != "test-admin-secret-key")
            {
                return Unauthorized(new { message = "Brak uprawnień administratora" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new { message = "Użytkownik nie został znaleziony" });
            }

            // Potwierdź email i zaloguj użytkownika
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { message = $"Użytkownik {user.Email} został autoryzowany dla testów" });
        }

        [HttpPost("admin/create-and-authorize-user")]
        public async Task<IActionResult> CreateAndAuthorizeUserForTesting([FromBody] RegisterDto model)
        {
            // Tylko dla testów - w produkcji to powinno być zabezpieczone
            if (!HttpContext.Request.Headers.ContainsKey("X-Test-Admin-Key") || 
                HttpContext.Request.Headers["X-Test-Admin-Key"] != "test-admin-secret-key")
            {
                return Unauthorized(new { message = "Brak uprawnień administratora" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdź czy użytkownik już istnieje
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                // Jeśli istnieje, po prostu go autoryzuj
                existingUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(existingUser);
                await _signInManager.SignInAsync(existingUser, isPersistent: false);
                return Ok(new { message = $"Istniejący użytkownik {existingUser.Email} został autoryzowany dla testów" });
            }

            // Utwórz nowego użytkownika
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Height = model.Height,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true // Od razu potwierdź email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Zaloguj użytkownika
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                // Dodaj informacje o użytkowniku do odpowiedzi
                return Ok(new { 
                    message = $"Nowy użytkownik {user.Email} został utworzony i autoryzowany dla testów",
                    userId = user.Id,
                    email = user.Email
                });
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }
    }
} 