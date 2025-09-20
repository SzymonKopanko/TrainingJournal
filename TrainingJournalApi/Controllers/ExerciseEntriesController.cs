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
    public class ExerciseEntriesController : ControllerBase
    {
        private readonly TrainingJournalApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseEntriesController(TrainingJournalApiContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExerciseEntryDto>>> GetExerciseEntries()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseEntries = await _context.ExerciseEntries
                .Include(e => e.Exercise)
                .Where(e => e.UserId == user.Id)
                .Select(e => new ExerciseEntryDto
                {
                    Id = e.Id,
                    ExerciseId = e.ExerciseId,
                    ExerciseName = e.Exercise.Name,
                    Notes = e.Notes,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            
            return Ok(exerciseEntries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseEntryDto>> GetExerciseEntryById(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseEntry = await _context.ExerciseEntries
                .Include(e => e.Exercise)
                .Where(e => e.Id == id && e.UserId == user.Id)
                .Select(e => new ExerciseEntryDto
                {
                    Id = e.Id,
                    ExerciseId = e.ExerciseId,
                    ExerciseName = e.Exercise.Name,
                    Notes = e.Notes,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .FirstOrDefaultAsync();
            
            if (exerciseEntry == null)
            {
                return NotFound();
            }
            return Ok(exerciseEntry);
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseEntryDto>> AddExerciseEntry([FromBody] CreateExerciseEntryDto createDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Sprawdź czy ćwiczenie istnieje i należy do użytkownika
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == createDto.ExerciseId && e.UserId == user.Id);
            
            if (exercise == null)
            {
                return BadRequest("Ćwiczenie nie istnieje lub nie należy do użytkownika.");
            }

            var exerciseEntry = new ExerciseEntry
            {
                ExerciseId = createDto.ExerciseId,
                Notes = createDto.Notes,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.ExerciseEntries.Add(exerciseEntry);
            await _context.SaveChangesAsync();
            
            var exerciseEntryDto = new ExerciseEntryDto
            {
                Id = exerciseEntry.Id,
                ExerciseId = exerciseEntry.ExerciseId,
                ExerciseName = exercise.Name,
                Notes = exerciseEntry.Notes,
                CreatedAt = exerciseEntry.CreatedAt,
                UpdatedAt = exerciseEntry.UpdatedAt
            };
            
            return CreatedAtAction(nameof(GetExerciseEntryById), new { id = exerciseEntry.Id }, exerciseEntryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExerciseEntry(int id, [FromBody] UpdateExerciseEntryDto updateDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseEntry = await _context.ExerciseEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
            
            if (exerciseEntry == null)
            {
                return NotFound();
            }

            exerciseEntry.Notes = updateDto.Notes;
            exerciseEntry.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(exerciseEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseEntry(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exerciseEntry = await _context.ExerciseEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
            
            if (exerciseEntry == null)
            {
                return NotFound();
            }

            _context.ExerciseEntries.Remove(exerciseEntry);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("exercise/{exerciseId}")]
        public async Task<ActionResult<List<ExerciseEntryDto>>> GetExerciseEntriesByExercise(int exerciseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Sprawdź czy ćwiczenie należy do użytkownika
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.UserId == user.Id);
            
            if (exercise == null)
            {
                return NotFound("Ćwiczenie nie istnieje lub nie należy do użytkownika.");
            }

            var exerciseEntries = await _context.ExerciseEntries
                .Include(e => e.Exercise)
                .Where(e => e.ExerciseId == exerciseId && e.UserId == user.Id)
                .Select(e => new ExerciseEntryDto
                {
                    Id = e.Id,
                    ExerciseId = e.ExerciseId,
                    ExerciseName = e.Exercise.Name,
                    Notes = e.Notes,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            
            return Ok(exerciseEntries);
        }
    }
} 