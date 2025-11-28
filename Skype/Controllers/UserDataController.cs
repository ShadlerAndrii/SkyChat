using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Skype.Constants;
using Skype.Data;
using Skype.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skype.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        RepositoryUsers _repository;

        IConfiguration _configuration;

        public UserDataController(RepositoryUsers repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        private string GenerateJwtToken(string id, string role, string username, string name)
        {
            var claims = new[]
            {
                new Claim("id", $"{id}"),
                new Claim(ClaimTypes.Role, $"{role}"),
                new Claim("username", $"{username}"),
                new Claim("name", $"{name}")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: loginCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        [HttpGet]
        [Authorize(Roles = "Support")]
        public async Task<List<User>> GetData()
        {
            return await _repository.GetUserData();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddData(   [FromForm] string name,
                                                    [FromForm] string username,
                                                    [FromForm] string phone,
                                                    [FromForm] string password,
                                                    [FromForm] UserRole role)
        {
            if (!await _repository.TryAddUserData(name, username, phone, password, role))
            {
                return Conflict("User already exist");
            }

            return Ok();
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser( [FromForm] string username,
                                                    [FromForm] string password)
        {
            var user = await _repository.LoginUser(username, password);

            if (user == null)
            {
                return Unauthorized();
            }

            var id = user.Id.ToString();
            var role = user.Role.ToString();
            var name = user.Name.ToString();

            var usernameNotLower = user.Username.ToString();

            var token = GenerateJwtToken(id, role, usernameNotLower, name);

            return Ok( new { token } );
        }
    }
}