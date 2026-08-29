using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext context;
        public CategoryRepository(AppDBContext context) => this.context = context;
       

        public async Task AddAsync(Category category)
        {
            await context.Categories.AddAsync(category);   
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync() =>
            await context.Categories
                .Include(c => c.Jobs)
                .OrderBy(c => c.Name)
                .ToListAsync();
       

        public async Task<Category?> GetByIdAsync(long id) =>
            await context.Categories
                .Include (c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == id);
        

        public async Task UpdateAsync(Category category)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }
    }
}
