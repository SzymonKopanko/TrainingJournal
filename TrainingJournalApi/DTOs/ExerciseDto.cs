using TrainingJournalApi.Models;

namespace TrainingJournalApi.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double BodyWeightPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroupDto>();
    }

    public class CreateExerciseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double BodyWeightPercentage { get; set; }
        public List<CreateExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<CreateExerciseMuscleGroupDto>();
    }

    public class UpdateExerciseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double BodyWeightPercentage { get; set; }
        public List<CreateExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<CreateExerciseMuscleGroupDto>();
    }
} 