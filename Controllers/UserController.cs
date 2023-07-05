using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Entities;
using ToDoBackend.Repositories;
using ToDoBackend.Services;

namespace ToDoBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _usersRepository;

        public UserController(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }

            await _usersRepository.CreateUserAsync(user);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return BadRequest("User with this email does not exist");
            }

            // Check if the password is correct
            if (!BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
            {
                return BadRequest("Invalid password");
            }

            // If the user exists and the password is correct, return a token
            var token = new TokenService().GenerateToken(existingUser.Email);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersRepository.GetAllUsersAsync();
        }


    }
}