using TrainingJournalApi.Models;
using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double BodyWeightPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroupDto>();
    }

    public class CreateExerciseDto
    {
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana")]
        [MinLength(2, ErrorMessage = "Nazwa ćwiczenia musi mieć co najmniej 2 znaki")]
        [MaxLength(100, ErrorMessage = "Nazwa ćwiczenia nie może przekraczać 100 znaków")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 2, ErrorMessage = "Procent masy ciała musi być między 0 a 2")]
        public double BodyWeightPercentage { get; set; }

        public List<CreateExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<CreateExerciseMuscleGroupDto>();
    }

    public class UpdateExerciseDto
    {
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana")]
        [MinLength(2, ErrorMessage = "Nazwa ćwiczenia musi mieć co najmniej 2 znaki")]
        [MaxLength(100, ErrorMessage = "Nazwa ćwiczenia nie może przekraczać 100 znaków")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 99, ErrorMessage = "Procent masy ciała musi być między 0 a 99")]
        public double BodyWeightPercentage { get; set; }

        public List<CreateExerciseMuscleGroupDto> ExerciseMuscleGroups { get; set; } = new List<CreateExerciseMuscleGroupDto>();
    }
} 