using GymPeriodisation.Application.RepositoryInterfaces;
using GymPeriodisation.Domain.Entities;
using GymPeriodisation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymPeriodisation.Infrastructure.Repositories
{
    public class MuscleRepository : IMuscleRepository
    {
        private readonly GymDbContext _context;

        public MuscleRepository(GymDbContext context)
        {
            _context = context;
        }
        public async Task<List<Muscle>> GetByIdsAsync(List<int> muscleGroupIds)
        {
            return await _context.Muscles
                .Where(m => muscleGroupIds.Contains(m.Id))
                .ToListAsync();
        }
    }
}
