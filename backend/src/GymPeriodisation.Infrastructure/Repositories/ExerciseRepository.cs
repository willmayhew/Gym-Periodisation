using GymPeriodisation.Application.RepositoryInterfaces;
using GymPeriodisation.Domain.Entities;
using GymPeriodisation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymPeriodisation.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly GymDbContext _context;

        public ExerciseRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise?> AddAsync(Exercise exercise)
        {
            var entry = await _context.Exercises.AddAsync(exercise);
            return entry.Entity;
        }

        public async Task<Exercise?> GetByIdAsync(int exerciseId)
        {
            return await _context.Exercises
                .Include(e => e.MuscleGroups)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);
        }

        public async Task<Exercise?> GetByNormalizedNameAsync(string name)
        {
            var normalizedName = Normalize(name);

            return await _context.Exercises
                .Include(e => e.MuscleGroups)
                .FirstOrDefaultAsync(e => String.Equals(e.NormalizedName, normalizedName));
        }
        private static string Normalize(string name)
        {
            return new string(name
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray())
                .ToLowerInvariant();
        }
    }
}
