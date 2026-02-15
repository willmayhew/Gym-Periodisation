using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.DTOs.Workouts
{
    public class WorkoutResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public DateTime DateStarted { get; set; }

        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }

}
