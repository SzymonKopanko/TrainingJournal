namespace TrainingJournalApi.Models
{
    public class ExerciseEntry
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; } = null!;
        public string Notes { get; set; } = string.Empty;
        
        // Relacja do użytkownika
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Relacja do serii ćwiczeń
        public virtual ICollection<ExerciseSet> ExerciseSets { get; set; } = new List<ExerciseSet>();
    }
} 