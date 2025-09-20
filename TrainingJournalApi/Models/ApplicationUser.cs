using Microsoft.AspNetCore.Identity;

namespace TrainingJournalApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public double Height { get; set; } // wzrost użytkownika w cm
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        
        // Relacja do ćwiczeń użytkownika
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        
        // Relacja do wpisów ćwiczeń użytkownika
        public virtual ICollection<ExerciseEntry> ExerciseEntries { get; set; } = new List<ExerciseEntry>();
        
        // Relacja do serii ćwiczeń użytkownika
        public virtual ICollection<ExerciseSet> ExerciseSets { get; set; } = new List<ExerciseSet>();
        
        // Relacja do historii wagi użytkownika
        public virtual ICollection<UserWeight> WeightHistory { get; set; } = new List<UserWeight>();
        
        // Relacja do treningów użytkownika
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
    }
} 