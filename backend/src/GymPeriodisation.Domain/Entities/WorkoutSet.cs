namespace GymPeriodisation.Domain.Entities;

public class WorkoutSet
{
    public int Id { get; set; }
    public int WorkoutExerciseId { get; set; }
    public WorkoutExercise WorkoutExercise { get; set; } = null!;
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int SetNumber { get; set; }
}
