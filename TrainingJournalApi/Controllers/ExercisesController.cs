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
    public class ExercisesController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExercisesController(TrainingJournalApiContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExerciseDto>>> GetExercises()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exercises = await _context.Exercises
                .Where(e => e.UserId == user.Id)
                .Include(e => e.ExerciseMuscleGroups)
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    BodyWeightPercentage = e.BodyWeightPercentage,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    ExerciseMuscleGroups = e.ExerciseMuscleGroups.Select(emg => new ExerciseMuscleGroupDto
                    {
                        Id = emg.Id,
                        ExerciseId = emg.ExerciseId,
                        MuscleGroup = emg.MuscleGroup,
                        Role = emg.Role,
                        CreatedAt = emg.CreatedAt,
                        UpdatedAt = emg.UpdatedAt
                    }).ToList()
                })
                .ToListAsync();
            
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetExerciseById(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exercise = await _context.Exercises
                .Where(e => e.Id == id && e.UserId == user.Id)
                .Include(e => e.ExerciseMuscleGroups)
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    BodyWeightPercentage = e.BodyWeightPercentage,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    ExerciseMuscleGroups = e.ExerciseMuscleGroups.Select(emg => new ExerciseMuscleGroupDto
                    {
                        Id = emg.Id,
                        ExerciseId = emg.ExerciseId,
                        MuscleGroup = emg.MuscleGroup,
                        Role = emg.Role,
                        CreatedAt = emg.CreatedAt,
                        UpdatedAt = emg.UpdatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> AddExercise([FromBody] CreateExerciseDto createDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(createDto.Name) || string.IsNullOrEmpty(createDto.Description))
            {
                return BadRequest("Invalid exercise data.");
            }

            var exercise = new Exercise
            {
                Name = createDto.Name,
                Description = createDto.Description,
                BodyWeightPercentage = createDto.BodyWeightPercentage,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            
            // Dodaj grupy mięśniowe
            if (createDto.ExerciseMuscleGroups != null && createDto.ExerciseMuscleGroups.Any())
            {
                foreach (var muscleGroupDto in createDto.ExerciseMuscleGroups)
                {
                    var exerciseMuscleGroup = new ExerciseMuscleGroup
                    {
                        ExerciseId = exercise.Id,
                        MuscleGroup = muscleGroupDto.MuscleGroup,
                        Role = muscleGroupDto.Role,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.ExerciseMuscleGroups.Add(exerciseMuscleGroup);
                }
                await _context.SaveChangesAsync();
            }
            
            // Pobierz utworzone ćwiczenie z grupami mięśniowymi
            var createdExercise = await _context.Exercises
                .Where(e => e.Id == exercise.Id)
                .Include(e => e.ExerciseMuscleGroups)
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    BodyWeightPercentage = e.BodyWeightPercentage,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    ExerciseMuscleGroups = e.ExerciseMuscleGroups.Select(emg => new ExerciseMuscleGroupDto
                    {
                        Id = emg.Id,
                        ExerciseId = emg.ExerciseId,
                        MuscleGroup = emg.MuscleGroup,
                        Role = emg.Role,
                        CreatedAt = emg.CreatedAt,
                        UpdatedAt = emg.UpdatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            
            return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, createdExercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] UpdateExerciseDto updateDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
            
            if (exercise == null)
            {
                return NotFound();
            }

            exercise.Name = updateDto.Name;
            exercise.Description = updateDto.Description;
            exercise.BodyWeightPercentage = updateDto.BodyWeightPercentage;
            exercise.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(exercise).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // Usuń istniejące grupy mięśniowe
            var existingMuscleGroups = await _context.ExerciseMuscleGroups
                .Where(emg => emg.ExerciseId == id)
                .ToListAsync();
            _context.ExerciseMuscleGroups.RemoveRange(existingMuscleGroups);
            
            // Dodaj nowe grupy mięśniowe
            if (updateDto.ExerciseMuscleGroups != null && updateDto.ExerciseMuscleGroups.Any())
            {
                foreach (var muscleGroupDto in updateDto.ExerciseMuscleGroups)
                {
                    var exerciseMuscleGroup = new ExerciseMuscleGroup
                    {
                        ExerciseId = id,
                        MuscleGroup = muscleGroupDto.MuscleGroup,
                        Role = muscleGroupDto.Role,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.ExerciseMuscleGroups.Add(exerciseMuscleGroup);
                }
            }
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
            
            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
