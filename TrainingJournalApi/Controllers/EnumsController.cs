using Microsoft.AspNetCore.Mvc;
using TrainingJournalApi.Models;

namespace TrainingJournalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumsController : ControllerBase
    {
        // GET: api/Enums/muscle-groups
        [HttpGet("muscle-groups")]
        public ActionResult<IEnumerable<object>> GetMuscleGroups()
        {
            var muscleGroups = Enum.GetValues(typeof(MuscleGroup))
                .Cast<MuscleGroup>()
                .Select(mg => new
                {
                    Value = (int)mg,
                    Name = mg.ToString(),
                    DisplayName = GetMuscleGroupDisplayName(mg)
                })
                .ToList();

            return Ok(muscleGroups);
        }

        // GET: api/Enums/muscle-group-roles
        [HttpGet("muscle-group-roles")]
        public ActionResult<IEnumerable<object>> GetMuscleGroupRoles()
        {
            var muscleGroupRoles = Enum.GetValues(typeof(MuscleGroupRole))
                .Cast<MuscleGroupRole>()
                .Select(mgr => new
                {
                    Value = (int)mgr,
                    Name = mgr.ToString(),
                    DisplayName = GetMuscleGroupRoleDisplayName(mgr)
                })
                .ToList();

            return Ok(muscleGroupRoles);
        }

        private static string GetMuscleGroupDisplayName(MuscleGroup muscleGroup)
        {
            return muscleGroup switch
            {
                MuscleGroup.Chest => "Klatka piersiowa",
                MuscleGroup.Back => "Plecy",
                MuscleGroup.FrontDeltoid => "Przedni akton barków",
                MuscleGroup.MiddleDeltoid => "Boczny akton barków",
                MuscleGroup.RearDeltoid => "Tylny akton barków",
                MuscleGroup.Biceps => "Biceps",
                MuscleGroup.Triceps => "Triceps",
                MuscleGroup.Quads => "Mięśnie czworogłowe ud",
                MuscleGroup.Hamstrings => "Mięśnie dwugłowe ud",
                MuscleGroup.Glutes => "Pośladki",
                MuscleGroup.Calves => "Łydki",
                MuscleGroup.Abs => "Brzuch",
                MuscleGroup.Forearms => "Przedramiona",
                MuscleGroup.Cardio => "Cardio",
                _ => muscleGroup.ToString()
            };
        }

        private static string GetMuscleGroupRoleDisplayName(MuscleGroupRole role)
        {
            return role switch
            {
                MuscleGroupRole.Primary => "Partia główna",
                MuscleGroupRole.Secondary => "Partia poboczna",
                _ => role.ToString()
            };
        }
    }
} 