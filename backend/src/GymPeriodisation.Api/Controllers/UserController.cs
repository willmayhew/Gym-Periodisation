using GymPeriodisation.Application.DTOs.Users;
using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Application.Services;
using GymPeriodisation.Application.Services.Interfaces;
using GymPeriodisation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymPeriodisation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            await _userService.CreateUser(user);
            return Ok(user);
        }
    }
}
