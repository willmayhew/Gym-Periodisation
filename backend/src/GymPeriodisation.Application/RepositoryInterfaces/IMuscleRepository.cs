using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Application.RepositoryInterfaces
{
    public interface IMuscleRepository
    {
        Task<List<Muscle>> GetByIdsAsync(List<int> muscleGroupIds);
    }
}
