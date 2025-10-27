using Microsoft.EntityFrameworkCore;
using VStore.ProductApi.Domain.IRepository;
using VStore.ProductApi.Domain.Models;
using VStore.ProductApi.Infrastructure.ProductContex;

namespace VStore.ProductApi.Infrastructure.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DbProductContext _context;

        public CategoryRepository(DbProductContext context)
        {
            _context = context;
        }

        public async Task<Category> Create(Category create)
        {
            await _context.Categories.AddAsync(create);
            await _context.SaveChangesAsync();
            return create;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await FindById(id);
            _context.Categories.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Category> FindById(int id)
        {
            var category = await _context.Categories.Include(x => x.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<List<Category>> FindByText(string query)
        {
            var categoryDb = await _context.Categories.Include(x => x.Products)
                .ToListAsync();
            return categoryDb;
            
        }

        public async Task<List<Category>> GetAll()
        {
            var categories = await _context.Categories.Include(x => x.Products)
                .ToListAsync();

            return categories;
        }

        public Task<List<Category>> GetProductsByIdsAsync(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Update(Category update)
        {
            _context.Entry(update).CurrentValues.SetValues(update);
            await _context.SaveChangesAsync();

            return update;
        }
    }
}
