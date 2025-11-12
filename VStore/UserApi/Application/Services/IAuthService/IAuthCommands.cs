using BookReviewManager.Domain.ModelsAutentication;
using UserApi.Domain.ModelsAutentication;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace UserApi.Application.Services.IAuthService
{
    public interface IAuthCommands
    {
        Task<ResultViewModel<string>> CreateUser(RegisterUser registerUser);
        Task<ResultViewModel<ResponseLogin>> LoginUser(Login login);
    }
}
