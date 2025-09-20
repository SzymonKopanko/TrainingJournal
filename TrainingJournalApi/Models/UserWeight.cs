using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.Models;

public class UserWeight
{
    public int Id { get; set; }
    
    // Relacja do użytkownika
    public string UserId { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    
    public double Weight { get; set; } // waga w kg
    
    public DateTime WeightedAt { get; set; } //Domyślnie DateTime.UtcNow, ale można nadpisać

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
} 