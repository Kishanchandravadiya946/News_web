using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Models.IRepositoryImpl
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Set<Article>()
                .Where(a => a.PublishedDate.Date == today && a.IsPublished) 
                 .OrderByDescending(a => a.LikeCount) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetCategorywiseArticle(int id)
        {
             var  articles =  _context.Set<Article>().OrderByDescending(a => a.PublishedDate).Where(a => a.CategoryId == id).ToList();
            return articles;
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Set<Article>().FindAsync(id);
        }

        public async Task<Article> GetArticleByArticleCategory(int category, int article)
        {
            return await _context.Set<Article>().FirstOrDefaultAsync(a => a.CategoryId == category && a.Id == article);
        }
        public async Task AddArticleAsync(Article article)
        {
            
            await _context.Set<Article>().AddAsync(article);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateArticleAsync(Article article)
        {
            _context.Set<Article>().Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await GetArticleByIdAsync(id);
            if (article != null)
            {
                _context.Set<Article>().Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Set<Article>().Where(a => a.IsPublished).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesByCategoryIdAsync(int categoryId)
        {
            return await _context.Set<Article>().Where(a => a.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetRecommendedArticlesAsync(int categoryId, int articleId, int count)
        {
            return await _context.Set<Article>()
                .Where(a => a.CategoryId == categoryId && a.Id != articleId)
                .OrderByDescending(a => a.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Article> GetLatestArticleAsync()
        {
            return await _context.Set<Article>()
                .OrderByDescending(a => a.PublishedDate)
                .FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<Article>> SearchArticlesByTitle(string title)
        {
            return await _context.Set<Article>()
                .Where(a => a.Title.Contains(title))
                .ToListAsync();
        }

    }
}
