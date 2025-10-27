using BookReviewManager.Domain.ModelsAutentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Domain.Interfaces.IAuthService;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ICreateUser _createUser;

        public AuthController(ICreateUser createUser)
        {
            _createUser = createUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]  RegisterUser registerUser)
        {
            var result = await _createUser.CreateUserAsync(registerUser);
            if (result.Status == "Ok")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
