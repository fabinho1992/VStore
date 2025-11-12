namespace UserApi.Domain.Interfaces.IRepository
{
    public interface IRepositoryUser
    {
        Task<List<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetById(Guid id);
        Task<bool> Delete(User user);
    }
}
