using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movie_mart_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace movie_mart_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager; // User manager
        private readonly IConfiguration _configuration; // Configuration for retreiving JWT-installment
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            // Check if user with same username already exists
            var existingUserWithUsername = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserWithUsername != null)
            {
                return BadRequest("Username is already taken");
            }

            // Check if user with same email already exists
            var existingUserWithEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserWithEmail != null)
            {
                return BadRequest("Email is already registered");
            }

            // Create new user
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            // Create user with password hash
            var result = await _userManager.CreateAsync(user, model.PasswordHash);

            if (result.Succeeded)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }



    }
}

