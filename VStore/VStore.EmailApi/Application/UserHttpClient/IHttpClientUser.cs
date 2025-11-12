using VStore.EmailApi.Application.Dtos;

namespace VStore.EmailApi.Application.UserHttpClient
{
    public interface IHttpClientUser
    {
        Task<UserResponseHttp> GetUserByIdAsync(Guid userId);
    }
}
