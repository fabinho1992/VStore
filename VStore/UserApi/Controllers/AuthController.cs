using BookReviewManager.Domain.ModelsAutentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.Services.IAuthService;
using UserApi.Domain.Interfaces.IAuthService;
using UserApi.Domain.Interfaces.IService;
using UserApi.Domain.ModelsAutentication;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICreateUser _createUser;
        private readonly IAuthCommands _authCommands;
        private readonly ICRUD _service;

        public AuthController(ICreateUser createUser, IAuthCommands authCommands, ICRUD service)
        {
            _createUser = createUser;
            _authCommands = authCommands;
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var result = await _authCommands.CreateUser(registerUser);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var result = await _authCommands.LoginUser(login);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _service.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetEmail([FromQuery] string email)
        {
            var result = await _service.GetByEmail(email);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string email)
        {
            var result = await _service.DeleteAsync(email);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.Message);
        }




    } 
}
