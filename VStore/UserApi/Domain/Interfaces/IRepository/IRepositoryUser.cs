namespace UserApi.Domain.Interfaces.IRepository
{
    public interface IRepositoryUser
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(Guid id);
    }
}
