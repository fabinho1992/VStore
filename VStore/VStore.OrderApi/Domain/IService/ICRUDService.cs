using VStore.OrderApi.Apllication_Order.Dtos;

namespace VStore.OrderApi.Domain.IService
{
    public interface ICRUDService<T, U> where T : class where U : class
    {
        Task<ResultViewModel<List<T>>> GetAll();
        Task<ResultViewModel<T>> FindById(int id);
        Task<ResultViewModel<List<T>>> FindByText(string query);
        Task<ResultViewModel<T>> Create(U create);
        Task<ResultViewModel<T>> Update(int id, U update);
        Task<ResultViewModel<bool>> Delete(int id);
        Task<ResultViewModel<List<T>>> GetByListConsumer(Guid id);
    }
}
