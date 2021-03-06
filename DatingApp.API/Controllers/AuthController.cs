using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data.Intefaces;
using DatingApp.API.Dtos;
using DatingApp.API.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDto)
        {
            //TODO Validate Request

            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userRegisterDto.Username))
                return BadRequest("Username Already Exists");

            var userToCreate = new User()
            {
                Username = userRegisterDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, userRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(claims),
                Expires= DateTime.Now.AddHours(1),
                SigningCredentials=creds

            };    
            var tokenHandler= new JwtSecurityTokenHandler();
            var token =tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token=tokenHandler.WriteToken(token)        
            });
             
        }
    }
}