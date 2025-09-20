using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.Models
{
    public class TrainingExercise
    {
        public int Id { get; set; }
        
        // Relacja do treningu
        public int TrainingId { get; set; }
        public Training Training { get; set; }
        
        // Relacja do ćwiczenia
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        
        // Kolejność ćwiczeń w treningu
        [Required]
        public int Order { get; set; }
        
        // Opcjonalne ustawienia dla konkretnego ćwiczenia w treningu
        [StringLength(200)]
        public string? Notes { get; set; }
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 