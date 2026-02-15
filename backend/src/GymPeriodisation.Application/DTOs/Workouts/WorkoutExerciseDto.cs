using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.DTOs.Workouts
{
    public class WorkoutExerciseDto
    {
        public List<int> MuscleGroupIds { get; set; } = new();
        public string Name { get; set; } = string.Empty;

        public List<WorkoutSetDto> Sets { get; set; } = new();
    }
}
