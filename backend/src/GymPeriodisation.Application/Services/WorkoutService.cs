using GymPeriodisation.Application.DTOs.Workouts;
using GymPeriodisation.Application.Exceptions;
using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Application.RepositoryInterfaces;
using GymPeriodisation.Application.Services.Interfaces;
using GymPeriodisation.Domain.Entities;

namespace GymPeriodisation.Application.Services;

/// <summary>
/// Manages workout-related operations
/// CRUD operations for workouts, retrieving workouts for users, etc.
/// </summary>
public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IMuscleRepository _muscleRepository;

    public WorkoutService(IWorkoutRepository workoutRepository, IExerciseRepository exerciseRepository, IMuscleRepository muscleRepository  )
    {
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
        _muscleRepository = muscleRepository;
    }

    /// <summary>
    /// Workout is created
    /// An active workout is determined via whether a date ended exists.
    /// </summary>
    /// <param name="workoutDto"></param>
    /// <returns></returns>
    public async Task StartWorkoutAsync(CreateWorkoutDto workoutDto)
    {
        var workout = new Workout
        {
            UserId = workoutDto.UserId,
            Name = workoutDto.Name,
            DateStarted = DateTime.Now,
            DateEnded = null,
        };

        await _workoutRepository.AddAsync(workout);
    }

    /// <summary>
    /// Workout is ended by setting a date end.
    /// </summary>
    public async Task EndWorkoutAsync(int id, EndWorkoutDto workoutDto)
    {
        var workout = await ValidateWorkoutAsync(id);

        workout.DateEnded = DateTime.UtcNow;
        workout.Comment = workoutDto.Comment;

        await _workoutRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all of a users active and past workouts.
    /// </summary>
    public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
    {
        return await _workoutRepository.GetByUserIdAsync(userId);
    }

    /// <summary>
    /// Saves the workout sent.
    /// Used for adding exercises to an active workout.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="workoutDto"></param>
    /// <returns></returns>
    public async Task SaveWorkoutAsync(int id, SaveWorkoutDto workoutDto)
    {
        var workout = await ValidateWorkoutAsync(id);

        workout.Name = workoutDto.Name;
        workout.Comment = workoutDto.Comment;

        workout.WorkoutExercises.Clear();

        foreach (var exerciseDto in workoutDto.Exercises)
        {
            // Try find existing exercise by normalized name
            var exercise = new Exercise
            {
                Name = exerciseDto.Name
            };

            exercise = await _exerciseRepository
                .GetByNameAsync(exercise.NormalizedName);

            // If not found → create it
            if (exercise == null)
            {
                var muscles = await _muscleRepository.GetByIdsAsync(exerciseDto.MuscleGroupIds);

                exercise = new Exercise
                {
                    Name = exerciseDto.Name,
                    MuscleGroups = muscles.ToList()
                };

                await _exerciseRepository.AddAsync(exercise);
            }

            var workoutExercise = new WorkoutExercise
            {
                Exercise = exercise
            };

            foreach (var setDto in exerciseDto.Sets)
            {
                workoutExercise.Sets.Add(new WorkoutSet
                {
                    Reps = setDto.Reps,
                    Weight = setDto.Weight,
                    SetNumber = setDto.SetNumber
                });
            }

            workout.WorkoutExercises.Add(workoutExercise);
        }

        await _workoutRepository.SaveChangesAsync();
    }


    /// <summary>
    /// Checks whether a workout exists and is active.
    /// </summary>
    private async Task<Workout> ValidateWorkoutAsync(int id)
    {
        var workout = await _workoutRepository.GetByIdAsync(id);

        if (workout == null)
        {
            throw new NotFoundException("Workout not found");
        }

        if (workout.DateEnded != null)
        {
            throw new ValidationException("Workout already ended");
        }

        return workout;
    }

    /// <summary>
    /// Gets a workout from the Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<WorkoutResponseDto?> GetWorkoutByIdAsync(int id)
    {
        var workout = await _workoutRepository.GetByIdAsync(id);

        if (workout == null)
        {
            return null;
        }

        return new WorkoutResponseDto
        {
            Id = workout.Id,
            Name = workout.Name,
            Comment = workout.Comment,
            DateStarted = workout.DateStarted,
            Exercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseDto
            {
                Name = we.Exercise.Name,
                MuscleGroupIds = we.Exercise.MuscleGroups.Select(mg => mg.Id).ToList(),
                Sets = we.Sets.Select(s => new WorkoutSetDto
                {
                    Id = s.Id,
                    Reps = s.Reps,
                    Weight = s.Weight,
                    SetNumber = s.SetNumber
                }).ToList()
            }).ToList()
        };
    }
}
