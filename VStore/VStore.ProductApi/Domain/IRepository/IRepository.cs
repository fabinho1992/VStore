namespace VStore.ProductApi.Domain.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> FindById(int id);
        Task<List<T>> FindByText(string query);
        Task<List<T>> GetProductsByIdsAsync(List<int> ids); 
        Task<T> Create(T create);
        Task<T> Update(T update);
        Task<bool> Delete(int id);
    }

}
