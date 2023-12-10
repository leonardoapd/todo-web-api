using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.DTOs.UserDTOs;
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
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository usersRepository, ITokenService tokenService)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
        }

       [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDTO userDto)
        {
            var existingUser = await _usersRepository.GetUserByEmailAsync(userDto.Email);

            if (existingUser != null)
            {
                return Conflict("User with this email already exists");
            }

            var user = new User
            {
                Email = userDto.Email,
                Password = userDto.Password,
                Name = userDto.Name
            };

            await _usersRepository.CreateUserAsync(user);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            User existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return NotFound("User with this email does not exist");
            }

            // Check if the password is correct
            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return BadRequest("Invalid password");
            }

            // If the user exists and the password is correct, return a token
            var token = _tokenService.GenerateToken(existingUser.Email);

            Response.Headers.Append("Authorization", $"Bearer {token}");

            return Ok(new { existingUser.Email, existingUser.Name });
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return Ok("Logged out successfully");
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token");
            }

            // Obtiene el email del usuario desde el token JWT utilizando el método GetEmailFromToken de ITokenService
            var email = _tokenService.GetEmailFromToken(token);

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token or user not found");
            }

            var user = await _usersRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userResponse = new UserResponseDTO
            {
                Email = user.Email,
                Name = user.Name,
                Photo = user.Photo,
                Bio = user.Bio,
                Phone = user.Phone
            };

            return Ok(userResponse);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] User user)
        {
            var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return NotFound("User with this email does not exist");
            }

            if (existingUser.Password is not null && !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return BadRequest("Invalid password");
            }

            user.Password = user.Password is not null ? BCrypt.Net.BCrypt.HashPassword(user.Password) : null;
            user.Id = existingUser.Id;
            await _usersRepository.UpdateUserAsync(user);
            return Ok();
        }

        [Authorize]
        [HttpPatch("me/photo")]
        public async Task<IActionResult> UpdateCurrentUserPhoto([FromBody] UserPhotoDTO userPhotoDTO)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token");
            }

            var email = _tokenService.GetEmailFromToken(token);

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token or user not found");
            }

            var user = await _usersRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Photo = userPhotoDTO.Photo;
            await _usersRepository.UpdateUserAsync(user);
            return Ok();
        }
    }
}