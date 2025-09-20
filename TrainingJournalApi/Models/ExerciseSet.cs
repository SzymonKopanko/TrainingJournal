using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.Models
{
    public class ExerciseSet
    {
        public int Id { get; set; }
        public int ExerciseEntryId { get; set; }
        public virtual ExerciseEntry ExerciseEntry { get; set; } = null!;
        
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        
        public int Order { get; set; } // kolejność w ExerciseEntry
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int RIR { get; set; } // reps in reserve
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}