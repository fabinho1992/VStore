using Microsoft.EntityFrameworkCore;
using VStore.ProductApi.Domain.IRepository;
using VStore.ProductApi.Domain.Models;
using VStore.ProductApi.Infrastructure.ProductContex;

namespace VStore.ProductApi.Infrastructure.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly DbProductContext _context;

        public ProductRepository(DbProductContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(Product create)
        {
            await _context.Products.AddAsync(create);
            await _context.SaveChangesAsync();
            return create;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var product = await FindById(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;

            }
            catch(Exception)
            {
                throw new Exception("Error deleting the product");
            }

        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await _context.Products.Include(p => p.Catergory)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<Product> FindById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Catergory)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<List<Product>> FindByText(string query)
        {
            return await _context.Products
                .Where(m => m.Name.Contains(query))
                .ToListAsync();
        }

        public Task<List<Product>> GetAll()
        {
            var products = _context.Products
                .Include(p => p.Catergory)
                .ToListAsync();
            return products;
        }

        public async Task<Product> Update(Product update)
        {
            _context.Entry(update).CurrentValues.SetValues(update);
            await _context.SaveChangesAsync();

            return update;

        }
    }
}
