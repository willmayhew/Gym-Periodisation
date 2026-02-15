using GymPeriodisation.Application.DTOs.Workouts;
using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Application.ServiceInterfaces;
using GymPeriodisation.Application.Services;
using GymPeriodisation.Domain.Entities;
using GymPeriodisation.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymPeriodisation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkout(CreateWorkoutDto workout)
        {
            await _workoutService.StartWorkoutAsync(workout);
            return Ok(workout);
        }

        [HttpPost("{id}/end")]
        public async Task<IActionResult> EndWorkout([FromRoute] int id, [FromBody] EndWorkoutDto workout)
        {
            await _workoutService.EndWorkoutAsync(id, workout);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWorkouts([FromRoute] int userId)
        {
            var workout = await _workoutService.GetUserWorkoutsAsync(userId);
            return Ok(workout);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SaveWorkout([FromRoute] int id, [FromBody] SaveWorkoutDto workout)
        {
            await _workoutService.SaveWorkoutAsync(id, workout);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById([FromRoute] int id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            return Ok(workout);
        }
    }
}
