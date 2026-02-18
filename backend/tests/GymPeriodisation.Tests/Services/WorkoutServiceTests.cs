using GymPeriodisation.Application.DTOs.Workouts;
using GymPeriodisation.Application.Exceptions;
using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Application.RepositoryInterfaces;
using GymPeriodisation.Application.Services;
using GymPeriodisation.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace GymPeriodisation.Tests.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepo = new();
        private readonly Mock<IExerciseRepository> _exerciseRepo = new();
        private readonly Mock<IMuscleRepository> _muscleRepo = new();

        private readonly WorkoutService _service;

        public WorkoutServiceTests()
        {
            _service = new WorkoutService(
                _workoutRepo.Object,
                _exerciseRepo.Object,
                _muscleRepo.Object);
        }

        [Fact]
        public async Task SaveWorkoutAsync_ReusesExistingExercise()
        {
            var workout = new Workout
            {
                Id = 1,
                DateEnded = null,
                WorkoutExercises = new List<WorkoutExercise>()
            };

            var existingExercise = new Exercise
            {
                Id = 5,
                Name = "Bench Press"
            };

            _workoutRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(workout);

            _exerciseRepo.Setup(r => r.GetByNormalizedNameAsync("benchpress"))
                .ReturnsAsync(existingExercise);

            var dto = new SaveWorkoutDto
            {
                Name = "Push Day",
                Exercises = new List<WorkoutExerciseDto>
            {
            new()
            {
                Name = "Bench Press",
                MuscleGroupIds = new List<int>(),
                Sets = new List<WorkoutSetDto>
                {
                    new() { Reps = 10, Weight = 100 }
                }
            }
        }
            };

            await _service.SaveWorkoutAsync(1, dto);

            Assert.Single(workout.WorkoutExercises);
            Assert.Equal(existingExercise, workout.WorkoutExercises.First().Exercise);

            _exerciseRepo.Verify(r => r.AddAsync(It.IsAny<Exercise>()), Times.Never);
            _workoutRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SaveWorkoutAsync_CreatesExercise_WhenMissing()
        {
            var workout = new Workout
            {
                Id = 1,
                DateEnded = null,
                WorkoutExercises = new List<WorkoutExercise>()
            };

            _workoutRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(workout);

            _exerciseRepo.Setup(r => r.GetByNormalizedNameAsync("New Exercise"))
                .ReturnsAsync((Exercise?)null);

            _muscleRepo.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Muscle> { new() { Id = 1 } });

            var dto = new SaveWorkoutDto
            {
                Name = "Test",
                Exercises = new List<WorkoutExerciseDto>
        {
            new()
            {
                Name = "New Exercise",
                MuscleGroupIds = new List<int> { 1 },
                Sets = new List<WorkoutSetDto>()
            }
        }
            };

            await _service.SaveWorkoutAsync(1, dto);

            _exerciseRepo.Verify(r =>
                r.AddAsync(It.Is<Exercise>(e => e.Name == "New Exercise")),
                Times.Once);
        }

        [Fact]
        public async Task SaveWorkoutAsync_Throws_WhenWorkoutEnded()
        {
            var workout = new Workout
            {
                Id = 1,
                DateEnded = DateTime.UtcNow
            };

            _workoutRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(workout);

            await Assert.ThrowsAsync<ValidationException>(() =>
                _service.SaveWorkoutAsync(1, new SaveWorkoutDto()));
        }

        [Fact]
        public async Task SaveWorkoutAsync_OverwriteExercises()
        {
            var workout = new Workout
            {
                Id = 1,
                Name = "Result Exercise",
                DateEnded = null,
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise()
                    {
                        Exercise = new Exercise { Name = "Overwrite Exercise" },
                        Sets = new List<WorkoutSet>
                        {
                            new() { Reps = 10, Weight = 100 }
                        }
                    },
                    new WorkoutExercise
                    {
                        Exercise = new Exercise { Name = "Remove this Exercise" },
                        Sets = new List<WorkoutSet>
                        {
                            new() { Reps = 12, Weight = 50 }
                        }
                    }
                }
            };

            _workoutRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(workout);

            _exerciseRepo.Setup(r => r.GetByNormalizedNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Exercise());

            var dto = new SaveWorkoutDto
            {
                Name = "Test",
                Exercises = new List<WorkoutExerciseDto>
                {
                    new WorkoutExerciseDto
                    {
                        Name = "Overwrite Exercise",
                        MuscleGroupIds = new List<int>(),
                        Sets = new List<WorkoutSetDto>
                        {
                            new() { Reps = 8, Weight = 80 }
                        }
                    }
                },
            };

            await _service.SaveWorkoutAsync(1, dto);

            var updatedExercise = workout.WorkoutExercises.Single();
            var updatedSet = updatedExercise.Sets.Single();

            Assert.Equal(8, updatedSet.Reps);
            Assert.Equal(80, updatedSet.Weight);
        }


    }

}
