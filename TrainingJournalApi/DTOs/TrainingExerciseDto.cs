namespace TrainingJournalApi.DTOs
{
    public class TrainingExerciseDto
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int ExerciseId { get; set; }
        public ExerciseDto Exercise { get; set; } = new ExerciseDto();
        public int Order { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateTrainingExerciseDto
    {
        public int ExerciseId { get; set; }
        public int Order { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateTrainingExerciseDto
    {
        public int Order { get; set; }
        public string? Notes { get; set; }
    }
} 