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
    public double Weight { get; set; }
    public DateTime? WeightedAt { get; set; } // Opcjonalne, domyślnie DateTime.UtcNow
}

public class UpdateUserWeightDto
{
    public double Weight { get; set; }
    public DateTime WeightedAt { get; set; }
} 