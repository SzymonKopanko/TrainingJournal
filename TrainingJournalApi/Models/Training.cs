using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.Models
{
    public class Training
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } // np. "Trening A", "Trening B"
        
        [StringLength(500)]
        public string Description { get; set; }
        
        // Relacja do użytkownika
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        // Relacja do ćwiczeń treningowych
        public List<TrainingExercise> TrainingExercises { get; set; } = new List<TrainingExercise>();
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 