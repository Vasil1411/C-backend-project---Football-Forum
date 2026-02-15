using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.JWT;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtTokenGenerator _jwtGenerator; 

        
        public UsersController(UserService userService, JwtTokenGenerator jwtGenerator)
        {
            _userService = userService;
            _jwtGenerator = jwtGenerator;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest dto)
        {
            try
            {
                _userService.Register(dto);
                return Ok("User registered successfully!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest dto)
        {
            var user = _userService.GetUserByUsername(dto.Username);

            if (user == null || user.Password != dto.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

           
            string token = _jwtGenerator.GenerateToken(user.Id, user.Username);

            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            bool success = _userService.DeleteUser(id, currentUserId);
            
            if (!success) return BadRequest("Cannot delete account.");
            
            return Ok("Account deleted.");
        }
    }
