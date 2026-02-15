using GymPeriodisation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.RepositoryInterfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise?> GetByIdAsync(int exerciseId);
        Task<Exercise?> GetByNameAsync(string name);
        Task<Exercise?> AddAsync(Exercise exercise);
    }
}
