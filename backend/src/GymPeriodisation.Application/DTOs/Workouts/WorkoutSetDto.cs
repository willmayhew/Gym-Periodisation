using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.DTOs.Workouts
{
    public class WorkoutSetDto
    {
        public int? Id { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public int SetNumber { get; set; }
    }
}
