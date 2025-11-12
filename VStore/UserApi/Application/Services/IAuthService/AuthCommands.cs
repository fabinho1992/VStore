using BookReviewManager.Domain.ModelsAutentication;
using UserApi.Domain.Interfaces.IAuthService;
using UserApi.Domain.ModelsAutentication;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace UserApi.Application.Services.IAuthService
{
    public class AuthCommands : IAuthCommands
    {
        private readonly ICreateUser _createUser;
        private readonly ILoginUser _loginUser;

        public AuthCommands(ICreateUser createUser, ILoginUser loginUser)
        {
            _createUser = createUser;
            _loginUser = loginUser;
        }

        public async Task<ResultViewModel<string>> CreateUser(RegisterUser registerUser)
        {
            var userRegister = await _createUser.CreateUserAsync(registerUser);
            if (userRegister.Status == "Erro" || userRegister.Status == "Error creating user")
            {
                return ResultViewModel<string>.Error(userRegister.Message);
            }
            return ResultViewModel<string>.Success(userRegister.Message);

        }

        public async Task<ResultViewModel<ResponseLogin>> LoginUser(Login login)
        {
            var loginUser = await _loginUser.LoginAsync(login);
            if (loginUser.Status == "Sucess 200")
            {
                return ResultViewModel<ResponseLogin>.Success(loginUser);
            }
            return ResultViewModel<ResponseLogin>.Error(loginUser.Message);

        }
    }
}
