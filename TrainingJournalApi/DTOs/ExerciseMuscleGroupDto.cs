using TrainingJournalApi.Models;

namespace TrainingJournalApi.DTOs
{
    public class ExerciseMuscleGroupDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public MuscleGroupRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateExerciseMuscleGroupDto
    {
        public MuscleGroup MuscleGroup { get; set; }
        public MuscleGroupRole Role { get; set; }
    }

    public class UpdateExerciseMuscleGroupDto
    {
        public MuscleGroup MuscleGroup { get; set; }
        public MuscleGroupRole Role { get; set; }
    }
} 