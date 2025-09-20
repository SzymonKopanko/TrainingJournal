namespace TrainingJournalApi.DTOs
{
    public class ExerciseEntryDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateExerciseEntryDto
    {
        public int ExerciseId { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateExerciseEntryDto
    {
        public string Notes { get; set; } = string.Empty;
    }
} 