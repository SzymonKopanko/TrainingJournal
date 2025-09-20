using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.DTOs;

public class UserWeightDto
{
    public int Id { get; set; }
    public double Weight { get; set; }
    public DateTime WeightedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateUserWeightDto
{
    [Required(ErrorMessage = "Waga jest wymagana")]
    [Range(20, 500, ErrorMessage = "Waga musi być między 20 a 500 kg")]
    public double Weight { get; set; }

    [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Data ważenia musi być między 1900 a 2100 rokiem")]
    public DateTime? WeightedAt { get; set; } // Opcjonalne, domyślnie DateTime.UtcNow
}

public class UpdateUserWeightDto
{
    [Required(ErrorMessage = "Waga jest wymagana")]
    [Range(20, 500, ErrorMessage = "Waga musi być między 20 a 500 kg")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Data ważenia jest wymagana")]
    [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Data ważenia musi być między 1900 a 2100 rokiem")]
    public DateTime WeightedAt { get; set; }
} 