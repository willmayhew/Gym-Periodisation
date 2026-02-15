namespace GymPeriodisation.Domain.Entities;

public class Exercise
{
    public int Id { get; set; }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetName(value);
    }

    public string NormalizedName { get; private set; } = string.Empty;
    public ICollection<Muscle> MuscleGroups { get; set; } = new List<Muscle>();
    public ICollection<WorkoutExercise> WorkoutExercise { get; set; } = new List<WorkoutExercise>();

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Exercise name cannot be empty.");

        _name = FormatName(name);
        NormalizedName = Normalize(name);
    }

    private static string FormatName(string name)
    {
        var words = name
            .Split(new[] { ' ', '-', '.', ',', '_', '!' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower());

        return string.Join(' ', words);
    }

    private static string Normalize(string name)
    {
        return new string(
            name
                .Where(char.IsLetterOrDigit)
                .Select(char.ToLowerInvariant)
                .ToArray()
        );
    }
}
