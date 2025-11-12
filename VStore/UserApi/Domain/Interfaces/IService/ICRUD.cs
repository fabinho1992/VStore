using UserApi.Application.Dtos;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace UserApi.Domain.Interfaces.IService
{
    public interface ICRUD
    {
        Task<ResultViewModel<UserResponse>> GetByEmail(string email);
        Task<ResultViewModel<UserResponse>> GetByIdAsync(Guid id);
        Task<ResultViewModel<bool>> DeleteAsync(string email);
        Task<ResultViewModel<List<UserResponse>>> GetAllAsync();
    }
}
