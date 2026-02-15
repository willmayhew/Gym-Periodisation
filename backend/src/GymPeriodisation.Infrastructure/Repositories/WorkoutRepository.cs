using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Domain.Entities;
using GymPeriodisation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymPeriodisation.Infrastructure.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly GymDbContext _context;

    public WorkoutRepository(GymDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Workout workout)
    {
        await _context.Workouts.AddAsync(workout);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Workout>> GetByUserIdAsync(int userId)
    {
        return await _context.Workouts
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(int workoutId)
    {
        return await _context.Workouts
            .Where(w => w.Id == workoutId)
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .ThenInclude(e => e.MuscleGroups)
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Sets)
            .FirstOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
