using System.ComponentModel.DataAnnotations;

namespace TrainingJournalApi.DTOs
{
    public class ExerciseSetDto
    {
        public int Id { get; set; }
        public int ExerciseEntryId { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public double OneRepMax { get; set; }        // Obliczane na bieżąco w kontrolerze
        public int RIR { get; set; }
        public double PercievedOneRepMax { get; set; } // Obliczane na bieżąco w kontrolerze
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateExerciseSetDto
    {
        [Required(ErrorMessage = "ID wpisu ćwiczenia jest wymagane")]
        public int ExerciseEntryId { get; set; }

        [Required(ErrorMessage = "Kolejność jest wymagana")]
        [Range(1, 50, ErrorMessage = "Kolejność musi być między 1 a 50")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Liczba powtórzeń jest wymagana")]
        [Range(1, 100, ErrorMessage = "Liczba powtórzeń musi być między 1 a 100")]
        public int Reps { get; set; }

        [Required(ErrorMessage = "Ciężar jest wymagany")]
        [Range(0, 1000, ErrorMessage = "Ciężar musi być między 0 a 1000 kg")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "RIR jest wymagane")]
        [Range(0, 10, ErrorMessage = "RIR musi być między 0 a 10")]
        public int RIR { get; set; }
    }

    public class UpdateExerciseSetDto
    {
        [Required(ErrorMessage = "Kolejność jest wymagana")]
        [Range(1, 50, ErrorMessage = "Kolejność musi być między 1 a 50")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Liczba powtórzeń jest wymagana")]
        [Range(1, 100, ErrorMessage = "Liczba powtórzeń musi być między 1 a 100")]
        public int Reps { get; set; }

        [Required(ErrorMessage = "Ciężar jest wymagany")]
        [Range(0, 1000, ErrorMessage = "Ciężar musi być między 0 a 1000 kg")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "RIR jest wymagane")]
        [Range(0, 10, ErrorMessage = "RIR musi być między 0 a 10")]
        public int RIR { get; set; }
    }
}