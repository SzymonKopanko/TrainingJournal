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
    public class TrainingsController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;

        public TrainingsController(TrainingJournalApiContext context)
        {
            _context = context;
        }

        // GET: api/Trainings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTrainings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var trainings = await _context.Trainings
                .Where(t => t.UserId == userId)
                .Include(t => t.TrainingExercises)
                    .ThenInclude(te => te.Exercise)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            var trainingDtos = trainings.Select(t => new TrainingDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                TrainingExercises = t.TrainingExercises
                    .OrderBy(te => te.Order)
                    .Select(te => new TrainingExerciseDto
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
                    }).ToList()
            });

            return Ok(trainingDtos);
        }

        // GET: api/Trainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDto>> GetTraining(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var training = await _context.Trainings
                .Include(t => t.TrainingExercises)
                    .ThenInclude(te => te.Exercise)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (training == null)
            {
                return NotFound();
            }

            var trainingDto = new TrainingDto
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                UserId = training.UserId,
                CreatedAt = training.CreatedAt,
                UpdatedAt = training.UpdatedAt,
                TrainingExercises = training.TrainingExercises
                    .OrderBy(te => te.Order)
                    .Select(te => new TrainingExerciseDto
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
                    }).ToList()
            };

            return Ok(trainingDto);
        }

        // POST: api/Trainings
        [HttpPost]
        public async Task<ActionResult<TrainingDto>> CreateTraining(CreateTrainingDto createTrainingDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var training = new Training
            {
                Name = createTrainingDto.Name,
                Description = createTrainingDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();

            var trainingDto = new TrainingDto
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                UserId = training.UserId,
                CreatedAt = training.CreatedAt,
                UpdatedAt = training.UpdatedAt,
                TrainingExercises = new List<TrainingExerciseDto>()
            };

            return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, trainingDto);
        }

        // PUT: api/Trainings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(int id, UpdateTrainingDto updateTrainingDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var training = await _context.Trainings
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (training == null)
            {
                return NotFound();
            }

            training.Name = updateTrainingDto.Name;
            training.Description = updateTrainingDto.Description;
            training.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExists(id))
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

        // DELETE: api/Trainings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var training = await _context.Trainings
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (training == null)
            {
                return NotFound();
            }

            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingExists(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.Trainings.Any(t => t.Id == id && t.UserId == userId);
        }
    }
} 