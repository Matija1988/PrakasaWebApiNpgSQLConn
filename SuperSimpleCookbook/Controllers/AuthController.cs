using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorService<Author, AuthorRecipe> _service;

        private readonly IConfiguration _configuration;

        public AuthController(IAuthorService<Author, AuthorRecipe> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> LogIn(AuthDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var resposne = await _service.GetAllAsync();

            var validateUser = resposne.Items
                .Where(u => u.Username!.Equals(request.Username)).FirstOrDefault();

            if (validateUser == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, validateUser.Password))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                { new Claim("Id", validateUser.Username),
                    new Claim(ClaimTypes.Role, validateUser.Role.Name)}),

                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),

                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(jwt);

        }

    }
}

