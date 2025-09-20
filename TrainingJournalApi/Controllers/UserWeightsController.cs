using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Data;
using TrainingJournalApi.Models;
using TrainingJournalApi.DTOs;

namespace TrainingJournalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserWeightsController : ControllerBase
{
    private readonly TrainingJournalApiContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserWeightsController(TrainingJournalApiContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/UserWeights
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWeightDto>>> GetUserWeights()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var weights = await _context.UserWeights
            .Where(uw => uw.UserId == user.Id)
            .OrderByDescending(uw => uw.WeightedAt)
            .Select(uw => new UserWeightDto
            {
                Id = uw.Id,
                Weight = uw.Weight,
                WeightedAt = uw.WeightedAt,
                CreatedAt = uw.CreatedAt,
                UpdatedAt = uw.UpdatedAt
            })
            .ToListAsync();

        return Ok(weights);
    }

    // GET: api/UserWeights/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserWeightDto>> GetUserWeight(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var weight = await _context.UserWeights
            .Where(uw => uw.Id == id && uw.UserId == user.Id)
            .Select(uw => new UserWeightDto
            {
                Id = uw.Id,
                Weight = uw.Weight,
                WeightedAt = uw.WeightedAt,
                CreatedAt = uw.CreatedAt,
                UpdatedAt = uw.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (weight == null)
            return NotFound();

        return Ok(weight);
    }

    // GET: api/UserWeights/latest
    [HttpGet("latest")]
    public async Task<ActionResult<UserWeightDto>> GetLatestWeight()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var latestWeight = await _context.UserWeights
            .Where(uw => uw.UserId == user.Id)
            .OrderByDescending(uw => uw.WeightedAt)
            .Select(uw => new UserWeightDto
            {
                Id = uw.Id,
                Weight = uw.Weight,
                WeightedAt = uw.WeightedAt,
                CreatedAt = uw.CreatedAt,
                UpdatedAt = uw.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (latestWeight == null)
            return NotFound();

        return Ok(latestWeight);
    }

    // POST: api/UserWeights
    [HttpPost]
    public async Task<ActionResult<UserWeightDto>> CreateUserWeight(CreateUserWeightDto createDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var weight = new UserWeight
        {
            UserId = user.Id,
            Weight = createDto.Weight,
            WeightedAt = createDto.WeightedAt ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserWeights.Add(weight);
        await _context.SaveChangesAsync();

        var weightDto = new UserWeightDto
        {
            Id = weight.Id,
            Weight = weight.Weight,
            WeightedAt = weight.WeightedAt,
            CreatedAt = weight.CreatedAt,
            UpdatedAt = weight.UpdatedAt
        };

        return CreatedAtAction(nameof(GetUserWeight), new { id = weight.Id }, weightDto);
    }

    // PUT: api/UserWeights/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserWeight(int id, UpdateUserWeightDto updateDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var weight = await _context.UserWeights
            .FirstOrDefaultAsync(uw => uw.Id == id && uw.UserId == user.Id);

        if (weight == null)
            return NotFound();

        weight.Weight = updateDto.Weight;
        weight.WeightedAt = updateDto.WeightedAt;
        weight.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/UserWeights/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserWeight(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var weight = await _context.UserWeights
            .FirstOrDefaultAsync(uw => uw.Id == id && uw.UserId == user.Id);

        if (weight == null)
            return NotFound();

        _context.UserWeights.Remove(weight);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 