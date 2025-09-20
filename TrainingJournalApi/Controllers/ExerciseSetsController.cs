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

        [HttpGet]
        public async Task<ActionResult<List<ExerciseSetDto>>> GetExerciseSets()
        {
            var user = await _userManager.GetUserAsync(User);
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