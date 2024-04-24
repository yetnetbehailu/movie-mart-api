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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            // Find user based on username
            var user = await _userManager.FindByNameAsync(model.UserName);

            // If username not found look for user by email
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }

            // If user found & password correct
            if (user != null && await _userManager.CheckPasswordAsync(user, model.PasswordHash))
            {
                // Convert user Id to string
                string userIdString = user.Id.ToString();

                // Create JWT claims with user ID and username
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userIdString),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                // Create key & credentials to generate JWT token
                var jwtKey = _configuration["Jwt:Key"]; //config in appsettings.json
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)); // Class used to sign the JWT token created using bytes from config setting "Jwt:Key"
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Signing credentials class, requires security key & an algorithm(HMAC (Hash-based Message Authentication Code))

                /* Create JWT token
                   JwtSecurityToken class represents the JWT token itself
                   issuer: The identity provider or authentication server
                   audience: Receiver of token, the client or application that will consume the token
                   claims: Statements about the user (e.g., user ID, username).
                   expires: Expiration date token considered invalid. Renew by signing in again 
                   signingCredentials: Used to sign the token, includes the security key & algorithm
                */
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                );

                // Return OK with token converted to a string
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            // Return 401 Unauthorized if login fails
            return Unauthorized("Invalid username, email or password");

        }

    }

}

