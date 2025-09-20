namespace TrainingJournalApi.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double BodyWeightPercentage { get; set; } // procent masy ciała podnoszonej podczas ćwiczenia
        
        // Relacja do użytkownika
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relacja do wpisów ćwiczeń
        public virtual ICollection<ExerciseEntry> ExerciseEntries { get; set; } = new List<ExerciseEntry>();
        
        // Relacja do grup mięśniowych
        public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
    }
}


