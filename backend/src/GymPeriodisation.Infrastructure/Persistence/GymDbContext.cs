using GymPeriodisation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymPeriodisation.Infrastructure.Persistence;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();
    public DbSet<Muscle> Muscles => Set<Muscle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API if needed
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Exercise>()
            .HasMany(e => e.MuscleGroups)
            .WithMany(m => m.Exercises)
            .UsingEntity(j => j.ToTable("ExerciseMuscles"));

        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.NormalizedName)
            .IsUnique();

    }
}
