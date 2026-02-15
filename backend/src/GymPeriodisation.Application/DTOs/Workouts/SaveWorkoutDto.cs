using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.DTOs.Workouts
{
    public class SaveWorkoutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }
}
