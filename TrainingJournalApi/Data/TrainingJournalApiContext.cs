using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingJournalApi.Models;
using Microsoft.AspNetCore.Identity;

namespace TrainingJournalApi.Data
{
    public class TrainingJournalApiContext : IdentityDbContext<ApplicationUser>
    {
        public TrainingJournalApiContext(DbContextOptions<TrainingJournalApiContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed użytkownika
            var hasher = new PasswordHasher<ApplicationUser>();
            var seedUser = new ApplicationUser
            {
                Id = "seed-user-id",
                UserName = "seeduser@example.com",
                NormalizedUserName = "SEEDUSER@EXAMPLE.COM",
                Email = "seeduser@example.com",
                NormalizedEmail = "SEEDUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "Seed",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 180,
                CreatedAt = DateTime.UtcNow,
                SecurityStamp = string.Empty
            };
            seedUser.PasswordHash = hasher.HashPassword(seedUser, "Test123!");
            modelBuilder.Entity<ApplicationUser>().HasData(seedUser);

            // Konfiguracja relacji między Exercise a ApplicationUser
            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.User)
                .WithMany(u => u.Exercises)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji między ExerciseEntry a ApplicationUser
            modelBuilder.Entity<ExerciseEntry>()
                .HasOne(e => e.User)
                .WithMany(u => u.ExerciseEntries)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji między ExerciseEntry a Exercise
            modelBuilder.Entity<ExerciseEntry>()
                .HasOne(e => e.Exercise)
                .WithMany(ex => ex.ExerciseEntries)
                .HasForeignKey(e => e.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracja relacji między ExerciseSet a ExerciseEntry
            modelBuilder.Entity<ExerciseSet>()
                .HasOne(e => e.ExerciseEntry)
                .WithMany(ee => ee.ExerciseSets)
                .HasForeignKey(e => e.ExerciseEntryId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Konfiguracja relacji między ExerciseSet a ApplicationUser
            modelBuilder.Entity<ExerciseSet>()
                .HasOne(e => e.User)
                .WithMany(u => u.ExerciseSets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Konfiguracja relacji między UserWeight a ApplicationUser
            modelBuilder.Entity<UserWeight>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.WeightHistory)
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Konfiguracja relacji między Training a ApplicationUser
            modelBuilder.Entity<Training>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trainings)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Konfiguracja relacji między TrainingExercise a Training
            modelBuilder.Entity<TrainingExercise>()
                .HasOne(te => te.Training)
                .WithMany(t => t.TrainingExercises)
                .HasForeignKey(te => te.TrainingId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Konfiguracja relacji między TrainingExercise a Exercise
            modelBuilder.Entity<TrainingExercise>()
                .HasOne(te => te.Exercise)
                .WithMany()
                .HasForeignKey(te => te.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Konfiguracja relacji między ExerciseMuscleGroup a Exercise
            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasOne(emg => emg.Exercise)
                .WithMany(e => e.ExerciseMuscleGroups)
                .HasForeignKey(emg => emg.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Dodanie danych początkowych dla ćwiczeń przypisanych do seedUser
            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { 
                    Id = 1, 
                    Name = "Przysiad", 
                    Description = "Ćwiczenie na nogi", 
                    BodyWeightPercentage = 0.5,
                    UserId = seedUser.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Exercise { 
                    Id = 2, 
                    Name = "Martwy ciąg", 
                    Description = "Ćwiczenie na plecy", 
                    BodyWeightPercentage = 0.6,
                    UserId = seedUser.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Exercise { 
                    Id = 3, 
                    Name = "Wyciskanie sztangi", 
                    Description = "Ćwiczenie na klatkę piersiową", 
                    BodyWeightPercentage = 0.4,
                    UserId = seedUser.Id,
                    CreatedAt = DateTime.UtcNow
                });
                
            // Dodanie początkowej wagi dla seedUser
            modelBuilder.Entity<UserWeight>().HasData(
                new UserWeight
                {
                    Id = 1,
                    UserId = seedUser.Id,
                    Weight = 80.0,
                    WeightedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
        }
        
        public DbSet<Exercise> Exercises { get; set; } = null!;
        public DbSet<ExerciseEntry> ExerciseEntries { get; set; } = null!;
        public DbSet<ExerciseSet> ExerciseSets { get; set; } = null!;
        public DbSet<UserWeight> UserWeights { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public DbSet<TrainingExercise> TrainingExercises { get; set; } = null!;
        public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = null!;
    }
}
