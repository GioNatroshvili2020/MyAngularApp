using System.Threading.Tasks;
using DatingApp.API.Data.Intefaces;
using DatingApp.API.Dtos;
using DatingApp.API.models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController :ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDto)
        {
            //TODO Validate Request
            userRegisterDto.Username= userRegisterDto.Username.ToLower();

            if(await _repo.UserExists(userRegisterDto.Username))
                return BadRequest("Username Already Exists");
            
            var userToCreate= new User()
            {
                Username=userRegisterDto.Username
            };
            var createdUser= await _repo.Register(userToCreate, userRegisterDto.Password);
            
            return StatusCode(201);
        }
    }
}