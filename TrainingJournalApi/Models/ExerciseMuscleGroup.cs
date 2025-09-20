using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.Models
{
    public class ExerciseMuscleGroup
    {
        public int Id { get; set; }
        
        // Relacja do ćwiczenia
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        
        // Grupa mięśniowa
        public MuscleGroup MuscleGroup { get; set; }
        
        // Rola grupy mięśniowej w ćwiczeniu
        public MuscleGroupRole Role { get; set; }
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 