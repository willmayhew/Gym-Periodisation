using GymPeriodisation.Application.DTOs.Users;
using GymPeriodisation.Application.Interfaces;
using GymPeriodisation.Application.Services.Interfaces;
using GymPeriodisation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Manages user-related operations
/// </summary>
namespace GymPeriodisation.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        public async Task<List<User>> GetUsers()
        {
            return await _userRepository.GetAllAsync();
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        public async Task CreateUser(CreateUserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username
            };

            await _userRepository.AddAsync(user);
        }
    }
}
