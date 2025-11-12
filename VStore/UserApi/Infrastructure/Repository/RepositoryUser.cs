using Microsoft.EntityFrameworkCore;
using UserApi.Domain;
using UserApi.Domain.Interfaces.IRepository;

namespace UserApi.Infrastructure.Repository
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly UserDbContext _context;

        public RepositoryUser(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<List<User>> GetAll()
        {
            var users = _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.UserName == email);
            return user;
        }

        public Task<User> GetById(Guid id)
        {
            var user = _context.Users.FirstOrDefaultAsync(e => e.Id == id.ToString());
            return user;
        }
    }
}
