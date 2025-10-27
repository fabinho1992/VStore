using Microsoft.EntityFrameworkCore;
using VStore.OrderApi.Domain.IRepository;
using VStore.OrderApi.Domain.Models;
using VStore.OrderApi.Infrastructure.OrderContext;

namespace VStore.OrderApi.Infrastructure.Repository
{
    public class OrderReposiitory : IRepositoryOrder<Order>
    {
        private readonly DbOrderContext _context;

        public OrderReposiitory(DbOrderContext context)
        {
            _context = context;
        }

        public async Task<Order> Create(Order create)
        {
            await _context.Orders.AddAsync(create);
            await _context.SaveChangesAsync();
            return create;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var product = await FindById(id);
                _context.Orders.Remove(product);
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {
                throw new Exception("Error deleting the product");
            }
        }

        public async Task<Order> FindById(int id)
        {
            var order = await _context.Orders.Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            return order;
        }

        public async Task<List<Order>> FindByText(string query)
        {
            return await _context.Orders
               .Where(m => m.CreatedDate.Day.ToString().Contains(query))
               .ToListAsync();
        }

        public Task<List<Order>> GetAll()
        {
            var order = _context.Orders
                .Include(p => p.Items)
                .ToListAsync();
            return order;
        }

        public Task<List<Order>> GetProductsByIdsAsync(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> Update(Order update)
        {
            _context.Entry(update).CurrentValues.SetValues(update);
            await _context.SaveChangesAsync();

            return update;
        }
    }
}
