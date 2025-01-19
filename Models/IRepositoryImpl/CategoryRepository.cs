using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Models.IRepositoryImpl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Set<Category>().ToListAsync();
        }
        public async Task<Category> GetCategoryIdByNameAsync(string name)
        {

            var category = _context.Set<Category>().FirstOrDefault(c => c.Name == name);
            return category;
          
        }



        // not yet
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Set<Category>().AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Set<Category>().Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Set<Category>().Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithArticlesAsync()
        {
            return await _context.Set<Category>()
                                 .Include(c => c.Articles)
                                 .ToListAsync();
        }
    }
}
