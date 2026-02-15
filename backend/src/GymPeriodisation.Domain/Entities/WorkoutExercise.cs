using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Domain.Entities
{
    public class WorkoutExercise
    {
        public int? Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        public int WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public List<WorkoutSet> Sets { get; set; } = new();
    }

}
