using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Data;
using TrainingJournalApi.DTOs;
using TrainingJournalApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TrainingJournalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingExercisesController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;

        public TrainingExercisesController(TrainingJournalApiContext context)
        {
            _context = context;
        }

        // GET: api/TrainingExercises/training/5
        [HttpGet("training/{trainingId}")]
        public async Task<ActionResult<IEnumerable<TrainingExerciseDto>>> GetTrainingExercises(int trainingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Sprawdź czy trening należy do użytkownika
            var training = await _context.Trainings
                .FirstOrDefaultAsync(t => t.Id == trainingId && t.UserId == userId);
                
            if (training == null)
            {
                return NotFound("Training not found or access denied");
            }

            var trainingExercises = await _context.TrainingExercises
                .Where(te => te.TrainingId == trainingId)
                .Include(te => te.Exercise)
                .OrderBy(te => te.Order)
                .ToListAsync();

            var trainingExerciseDtos = trainingExercises.Select(te => new TrainingExerciseDto
            {
                Id = te.Id,
                TrainingId = te.TrainingId,
                ExerciseId = te.ExerciseId,
                Exercise = new ExerciseDto
                {
                    Id = te.Exercise.Id,
                    Name = te.Exercise.Name,
                    Description = te.Exercise.Description,
                    BodyWeightPercentage = te.Exercise.BodyWeightPercentage,
                    CreatedAt = te.Exercise.CreatedAt
                },
                Order = te.Order,
                Notes = te.Notes,
                CreatedAt = te.CreatedAt,
                UpdatedAt = te.UpdatedAt
            });

            return Ok(trainingExerciseDtos);
        }

        // GET: api/TrainingExercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingExerciseDto>> GetTrainingExercise(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var trainingExercise = await _context.TrainingExercises
                .Include(te => te.Exercise)
                .Include(te => te.Training)
                .FirstOrDefaultAsync(te => te.Id == id && te.Training.UserId == userId);

            if (trainingExercise == null)
            {
                return NotFound();
            }

            var trainingExerciseDto = new TrainingExerciseDto
            {
                Id = trainingExercise.Id,
                TrainingId = trainingExercise.TrainingId,
                ExerciseId = trainingExercise.ExerciseId,
                Exercise = new ExerciseDto
                {
                    Id = trainingExercise.Exercise.Id,
                    Name = trainingExercise.Exercise.Name,
                    Description = trainingExercise.Exercise.Description,
                    BodyWeightPercentage = trainingExercise.Exercise.BodyWeightPercentage,
                    CreatedAt = trainingExercise.Exercise.CreatedAt
                },
                Order = trainingExercise.Order,
                Notes = trainingExercise.Notes,
                CreatedAt = trainingExercise.CreatedAt,
                UpdatedAt = trainingExercise.UpdatedAt
            };

            return Ok(trainingExerciseDto);
        }

        // POST: api/TrainingExercises/training/5
        [HttpPost("training/{trainingId}")]
        public async Task<ActionResult<TrainingExerciseDto>> AddExerciseToTraining(int trainingId, CreateTrainingExerciseDto createTrainingExerciseDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Sprawdź czy trening należy do użytkownika
            var training = await _context.Trainings
                .FirstOrDefaultAsync(t => t.Id == trainingId && t.UserId == userId);
                
            if (training == null)
            {
                return NotFound("Training not found or access denied");
            }

            // Sprawdź czy ćwiczenie istnieje i należy do użytkownika
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == createTrainingExerciseDto.ExerciseId && e.UserId == userId);
                
            if (exercise == null)
            {
                return NotFound("Exercise not found or access denied");
            }

            // Sprawdź czy ćwiczenie już nie jest w tym treningu
            var existingExercise = await _context.TrainingExercises
                .FirstOrDefaultAsync(te => te.TrainingId == trainingId && te.ExerciseId == createTrainingExerciseDto.ExerciseId);
                
            if (existingExercise != null)
            {
                return BadRequest("Exercise is already in this training");
            }

            var trainingExercise = new TrainingExercise
            {
                TrainingId = trainingId,
                ExerciseId = createTrainingExerciseDto.ExerciseId,
                Order = createTrainingExerciseDto.Order,
                Notes = createTrainingExerciseDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.TrainingExercises.Add(trainingExercise);
            await _context.SaveChangesAsync();

            // Pobierz utworzone ćwiczenie z relacjami
            var createdTrainingExercise = await _context.TrainingExercises
                .Include(te => te.Exercise)
                .FirstOrDefaultAsync(te => te.Id == trainingExercise.Id);

            var trainingExerciseDto = new TrainingExerciseDto
            {
                Id = createdTrainingExercise.Id,
                TrainingId = createdTrainingExercise.TrainingId,
                ExerciseId = createdTrainingExercise.ExerciseId,
                Exercise = new ExerciseDto
                {
                    Id = createdTrainingExercise.Exercise.Id,
                    Name = createdTrainingExercise.Exercise.Name,
                    Description = createdTrainingExercise.Exercise.Description,
                    BodyWeightPercentage = createdTrainingExercise.Exercise.BodyWeightPercentage,
                    CreatedAt = createdTrainingExercise.Exercise.CreatedAt
                },
                Order = createdTrainingExercise.Order,
                Notes = createdTrainingExercise.Notes,
                CreatedAt = createdTrainingExercise.CreatedAt,
                UpdatedAt = createdTrainingExercise.UpdatedAt
            };

            return CreatedAtAction(nameof(GetTrainingExercise), new { id = trainingExercise.Id }, trainingExerciseDto);
        }

        // PUT: api/TrainingExercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingExercise(int id, UpdateTrainingExerciseDto updateTrainingExerciseDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var trainingExercise = await _context.TrainingExercises
                .Include(te => te.Training)
                .FirstOrDefaultAsync(te => te.Id == id && te.Training.UserId == userId);

            if (trainingExercise == null)
            {
                return NotFound();
            }

            trainingExercise.Order = updateTrainingExerciseDto.Order;
            trainingExercise.Notes = updateTrainingExerciseDto.Notes;
            trainingExercise.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TrainingExercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveExerciseFromTraining(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var trainingExercise = await _context.TrainingExercises
                .Include(te => te.Training)
                .FirstOrDefaultAsync(te => te.Id == id && te.Training.UserId == userId);

            if (trainingExercise == null)
            {
                return NotFound();
            }

            _context.TrainingExercises.Remove(trainingExercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingExerciseExists(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.TrainingExercises
                .Include(te => te.Training)
                .Any(te => te.Id == id && te.Training.UserId == userId);
        }
    }
} 