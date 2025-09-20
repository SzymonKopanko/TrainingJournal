using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Data;
using TrainingJournalApi.Models;
using TrainingJournalApi.DTOs;

namespace TrainingJournalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseSetsController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseSetsController(TrainingJournalApiContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            Console.WriteLine($"GetCurrentUserAsync called. User.Identity: {User.Identity?.Name}, IsAuthenticated: {User.Identity?.IsAuthenticated}, AuthenticationType: {User.Identity?.AuthenticationType}");
            
            // Najpierw spróbuj standardowej autoryzacji
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Console.WriteLine($"Found user via UserManager: {user.Email}");
                return user;
            }

            // Jeśli nie ma standardowej autoryzacji, sprawdź czy jest TestAuthHandler
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.AuthenticationType == "TestScheme")
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine($"Using test user ID from TestAuthHandler: {userId}");
                    // Dla testów, zwróć ApplicationUser z userId z TestAuthHandler
                    return new ApplicationUser { Id = userId };
                }
            }

            Console.WriteLine("No user found, returning null");
            return null;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExerciseSetDto>>> GetExerciseSets()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseSets = await _context.ExerciseSets
                .Include(e => e.ExerciseEntry)
                .ThenInclude(ee => ee.Exercise)
                .Where(e => e.UserId == user.Id)
                .OrderBy(e => e.ExerciseEntryId)
                .ThenBy(e => e.Order)
                .ToListAsync();

            var exerciseSetDtos = new List<ExerciseSetDto>();

            foreach (var exerciseSet in exerciseSets)
            {
                // Pobieramy wagę użytkownika z najbliższego czasu przed wykonaniem ExerciseEntry
                var userWeightAtTime = await _context.UserWeights
                    .Where(uw => uw.UserId == user.Id && uw.WeightedAt <= exerciseSet.ExerciseEntry.CreatedAt)
                    .OrderByDescending(uw => uw.WeightedAt)
                    .FirstOrDefaultAsync();

                var userWeight = userWeightAtTime?.Weight ?? 0.0; // Domyślnie 0 jeśli brak wagi

                exerciseSetDtos.Add(new ExerciseSetDto
                {
                    Id = exerciseSet.Id,
                    ExerciseEntryId = exerciseSet.ExerciseEntryId,
                    Order = exerciseSet.Order,
                    Reps = exerciseSet.Reps,
                    Weight = exerciseSet.Weight,
                    // Obliczamy 1RM na bieżąco używając wagi z czasu wykonywania ćwiczenia
                    OneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                    RIR = exerciseSet.RIR,
                    // Obliczamy perceived 1RM na bieżąco używając wagi z czasu wykonywania ćwiczenia
                    PercievedOneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps + exerciseSet.RIR, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                    CreatedAt = exerciseSet.CreatedAt,
                    UpdatedAt = exerciseSet.UpdatedAt
                });
            }
            
            return Ok(exerciseSetDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseSetDto>> GetExerciseSet(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseSet = await _context.ExerciseSets
                .Include(e => e.ExerciseEntry)
                .ThenInclude(ee => ee.Exercise)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            // Pobieramy wagę użytkownika z najbliższego czasu przed wykonaniem ExerciseEntry
            var userWeightAtTime = await _context.UserWeights
                .Where(uw => uw.UserId == user.Id && uw.WeightedAt <= exerciseSet.ExerciseEntry.CreatedAt)
                .OrderByDescending(uw => uw.WeightedAt)
                .FirstOrDefaultAsync();

            var userWeight = userWeightAtTime?.Weight ?? 0.0;

            var exerciseSetDto = new ExerciseSetDto
            {
                Id = exerciseSet.Id,
                ExerciseEntryId = exerciseSet.ExerciseEntryId,
                Order = exerciseSet.Order,
                Reps = exerciseSet.Reps,
                Weight = exerciseSet.Weight,
                OneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                RIR = exerciseSet.RIR,
                PercievedOneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps + exerciseSet.RIR, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                CreatedAt = exerciseSet.CreatedAt,
                UpdatedAt = exerciseSet.UpdatedAt
            };

            return Ok(exerciseSetDto);
        }

        [HttpGet("test")]
        public async Task<ActionResult<List<ExerciseSetDto>>> GetExerciseSetsForTesting()
        {
            // Tylko dla testów - użyj userId z headera
            if (!Request.Headers.ContainsKey("X-Test-User-Id"))
            {
                return Unauthorized();
            }

            var userId = Request.Headers["X-Test-User-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var exerciseSets = await _context.ExerciseSets
                .Include(e => e.ExerciseEntry)
                .ThenInclude(ee => ee.Exercise)
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.ExerciseEntryId)
                .ThenBy(e => e.Order)
                .ToListAsync();

            var exerciseSetDtos = new List<ExerciseSetDto>();

            foreach (var exerciseSet in exerciseSets)
            {
                // Pobieramy wagę użytkownika z najbliższego czasu przed wykonaniem ExerciseEntry
                var userWeightAtTime = await _context.UserWeights
                    .Where(uw => uw.UserId == userId && uw.WeightedAt <= exerciseSet.ExerciseEntry.CreatedAt)
                    .OrderByDescending(uw => uw.WeightedAt)
                    .FirstOrDefaultAsync();

                var userWeight = userWeightAtTime?.Weight ?? 0.0;

                exerciseSetDtos.Add(new ExerciseSetDto
                {
                    Id = exerciseSet.Id,
                    ExerciseEntryId = exerciseSet.ExerciseEntryId,
                    Order = exerciseSet.Order,
                    Reps = exerciseSet.Reps,
                    Weight = exerciseSet.Weight,
                    OneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                    RIR = exerciseSet.RIR,
                    PercievedOneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps + exerciseSet.RIR, userWeight, exerciseSet.ExerciseEntry.Exercise.BodyWeightPercentage),
                    CreatedAt = exerciseSet.CreatedAt,
                    UpdatedAt = exerciseSet.UpdatedAt
                });
            }
            
            return Ok(exerciseSetDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseSetDto>> CreateExerciseSet(CreateExerciseSetDto createDto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            // Sprawdzamy czy ExerciseEntry istnieje i należy do użytkownika
            var exerciseEntry = await _context.ExerciseEntries
                .Include(ee => ee.Exercise)
                .FirstOrDefaultAsync(ee => ee.Id == createDto.ExerciseEntryId && ee.UserId == user.Id);

            Console.WriteLine($"Looking for ExerciseEntry with ID: {createDto.ExerciseEntryId} and UserId: {user.Id}");
            Console.WriteLine($"Found ExerciseEntry: {(exerciseEntry != null ? exerciseEntry.Id.ToString() : "null")}");
            
            if (exerciseEntry == null)
            {
                // List all ExerciseEntries to debug
                var allEntries = await _context.ExerciseEntries.ToListAsync();
                Console.WriteLine($"All ExerciseEntries in database: {allEntries.Count}");
                foreach (var entry in allEntries)
                {
                    Console.WriteLine($"  Entry ID: {entry.Id}, UserId: {entry.UserId}");
                }
                return BadRequest("ExerciseEntry not found or doesn't belong to user");
            }

            var exerciseSet = new ExerciseSet
            {
                ExerciseEntryId = createDto.ExerciseEntryId,
                UserId = user.Id,
                Order = createDto.Order,
                Reps = createDto.Reps,
                Weight = createDto.Weight,
                RIR = createDto.RIR,
                CreatedAt = DateTime.UtcNow
            };

            _context.ExerciseSets.Add(exerciseSet);
            await _context.SaveChangesAsync();

            // Pobieramy wagę użytkownika do obliczeń
            var userWeightAtTime = await _context.UserWeights
                .Where(uw => uw.UserId == user.Id && uw.WeightedAt <= exerciseSet.CreatedAt)
                .OrderByDescending(uw => uw.WeightedAt)
                .FirstOrDefaultAsync();

            var userWeight = userWeightAtTime?.Weight ?? 0.0;

            var exerciseSetDto = new ExerciseSetDto
            {
                Id = exerciseSet.Id,
                ExerciseEntryId = exerciseSet.ExerciseEntryId,
                Order = exerciseSet.Order,
                Reps = exerciseSet.Reps,
                Weight = exerciseSet.Weight,
                OneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps, userWeight, exerciseEntry.Exercise.BodyWeightPercentage),
                RIR = exerciseSet.RIR,
                PercievedOneRepMax = CalculateOneRepMax(exerciseSet.Weight, exerciseSet.Reps + exerciseSet.RIR, userWeight, exerciseEntry.Exercise.BodyWeightPercentage),
                CreatedAt = exerciseSet.CreatedAt,
                UpdatedAt = exerciseSet.UpdatedAt
            };

            return CreatedAtAction(nameof(GetExerciseSet), new { id = exerciseSet.Id }, exerciseSetDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExerciseSet(int id, UpdateExerciseSetDto updateDto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseSet = await _context.ExerciseSets
                .Include(e => e.ExerciseEntry)
                .ThenInclude(ee => ee.Exercise)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            exerciseSet.Order = updateDto.Order;
            exerciseSet.Reps = updateDto.Reps;
            exerciseSet.Weight = updateDto.Weight;
            exerciseSet.RIR = updateDto.RIR;
            exerciseSet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseSet(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseSet = await _context.ExerciseSets
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            _context.ExerciseSets.Remove(exerciseSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Metoda pomocnicza do obliczania 1RM (wzór Brzyckiego z uwzględnieniem masy ciała)
        private static double CalculateOneRepMax(double weight, int reps, double userWeight, double bodyWeightPercentage)
        {
            // Obliczamy całkowity ciężar (dodatkowy + procent masy ciała)
            double totalWeight = weight + (userWeight * bodyWeightPercentage);
            if (reps <= 0) return totalWeight;
            if (reps == 1) return totalWeight;
            // Wzór Brzyckiego: 1RM = totalWeight * (36 / (37 - reps)) - (userWeight * bodyWeightPercentage)
            return totalWeight * (36.0 / (37.0 - reps)) - (userWeight * bodyWeightPercentage);
        }
    }
}