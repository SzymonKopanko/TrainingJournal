namespace TrainingJournalApi.DTOs
{
    public class ExerciseSetDto
    {
        public int Id { get; set; }
        public int ExerciseEntryId { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public double OneRepMax { get; set; }        // Obliczane na bieżąco w kontrolerze
        public int RIR { get; set; }
        public double PercievedOneRepMax { get; set; } // Obliczane na bieżąco w kontrolerze
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateExerciseSetDto
    {
        public int ExerciseEntryId { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int RIR { get; set; }
    }

    public class UpdateExerciseSetDto
    {
        public int Order { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int RIR { get; set; }
    }
}