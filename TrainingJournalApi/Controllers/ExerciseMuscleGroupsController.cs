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
    public class ExerciseMuscleGroupsController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;

        public ExerciseMuscleGroupsController(TrainingJournalApiContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseMuscleGroups/exercise/5
        [HttpGet("exercise/{exerciseId}")]
        public async Task<ActionResult<IEnumerable<ExerciseMuscleGroupDto>>> GetExerciseMuscleGroups(int exerciseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Sprawdź czy ćwiczenie należy do użytkownika
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.UserId == userId);
                
            if (exercise == null)
            {
                return NotFound("Exercise not found or access denied");
            }

            var exerciseMuscleGroups = await _context.ExerciseMuscleGroups
                .Where(emg => emg.ExerciseId == exerciseId)
                .OrderBy(emg => emg.Role)
                .ThenBy(emg => emg.MuscleGroup)
                .ToListAsync();

            var exerciseMuscleGroupDtos = exerciseMuscleGroups.Select(emg => new ExerciseMuscleGroupDto
            {
                Id = emg.Id,
                ExerciseId = emg.ExerciseId,
                MuscleGroup = emg.MuscleGroup,
                Role = emg.Role,
                CreatedAt = emg.CreatedAt,
                UpdatedAt = emg.UpdatedAt
            });

            return Ok(exerciseMuscleGroupDtos);
        }

        // GET: api/ExerciseMuscleGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseMuscleGroupDto>> GetExerciseMuscleGroup(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var exerciseMuscleGroup = await _context.ExerciseMuscleGroups
                .Include(emg => emg.Exercise)
                .FirstOrDefaultAsync(emg => emg.Id == id && emg.Exercise.UserId == userId);

            if (exerciseMuscleGroup == null)
            {
                return NotFound();
            }

            var exerciseMuscleGroupDto = new ExerciseMuscleGroupDto
            {
                Id = exerciseMuscleGroup.Id,
                ExerciseId = exerciseMuscleGroup.ExerciseId,
                MuscleGroup = exerciseMuscleGroup.MuscleGroup,
                Role = exerciseMuscleGroup.Role,
                CreatedAt = exerciseMuscleGroup.CreatedAt,
                UpdatedAt = exerciseMuscleGroup.UpdatedAt
            };

            return Ok(exerciseMuscleGroupDto);
        }

        // POST: api/ExerciseMuscleGroups/exercise/5
        [HttpPost("exercise/{exerciseId}")]
        public async Task<ActionResult<ExerciseMuscleGroupDto>> AddMuscleGroupToExercise(int exerciseId, CreateExerciseMuscleGroupDto createExerciseMuscleGroupDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Sprawdź czy ćwiczenie należy do użytkownika
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.UserId == userId);
                
            if (exercise == null)
            {
                return NotFound("Exercise not found or access denied");
            }

            // Sprawdź czy ta grupa mięśniowa już nie jest przypisana do tego ćwiczenia
            var existingMuscleGroup = await _context.ExerciseMuscleGroups
                .FirstOrDefaultAsync(emg => emg.ExerciseId == exerciseId && 
                                           emg.MuscleGroup == createExerciseMuscleGroupDto.MuscleGroup);
                
            if (existingMuscleGroup != null)
            {
                return BadRequest("This muscle group is already assigned to this exercise");
            }

            var exerciseMuscleGroup = new ExerciseMuscleGroup
            {
                ExerciseId = exerciseId,
                MuscleGroup = createExerciseMuscleGroupDto.MuscleGroup,
                Role = createExerciseMuscleGroupDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.ExerciseMuscleGroups.Add(exerciseMuscleGroup);
            await _context.SaveChangesAsync();

            var exerciseMuscleGroupDto = new ExerciseMuscleGroupDto
            {
                Id = exerciseMuscleGroup.Id,
                ExerciseId = exerciseMuscleGroup.ExerciseId,
                MuscleGroup = exerciseMuscleGroup.MuscleGroup,
                Role = exerciseMuscleGroup.Role,
                CreatedAt = exerciseMuscleGroup.CreatedAt,
                UpdatedAt = exerciseMuscleGroup.UpdatedAt
            };

            return CreatedAtAction(nameof(GetExerciseMuscleGroup), new { id = exerciseMuscleGroup.Id }, exerciseMuscleGroupDto);
        }

        // PUT: api/ExerciseMuscleGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExerciseMuscleGroup(int id, UpdateExerciseMuscleGroupDto updateExerciseMuscleGroupDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var exerciseMuscleGroup = await _context.ExerciseMuscleGroups
                .Include(emg => emg.Exercise)
                .FirstOrDefaultAsync(emg => emg.Id == id && emg.Exercise.UserId == userId);

            if (exerciseMuscleGroup == null)
            {
                return NotFound();
            }

            exerciseMuscleGroup.MuscleGroup = updateExerciseMuscleGroupDto.MuscleGroup;
            exerciseMuscleGroup.Role = updateExerciseMuscleGroupDto.Role;
            exerciseMuscleGroup.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseMuscleGroupExists(id))
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

        // DELETE: api/ExerciseMuscleGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMuscleGroupFromExercise(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var exerciseMuscleGroup = await _context.ExerciseMuscleGroups
                .Include(emg => emg.Exercise)
                .FirstOrDefaultAsync(emg => emg.Id == id && emg.Exercise.UserId == userId);

            if (exerciseMuscleGroup == null)
            {
                return NotFound();
            }

            _context.ExerciseMuscleGroups.Remove(exerciseMuscleGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseMuscleGroupExists(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.ExerciseMuscleGroups
                .Include(emg => emg.Exercise)
                .Any(emg => emg.Id == id && emg.Exercise.UserId == userId);
        }
    }
} 