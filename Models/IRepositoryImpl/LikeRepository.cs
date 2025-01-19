using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Models.IRepositoryImpl
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetAllLikesAsync()
        {
            return await _context.Set<Like>().ToListAsync();
        }

        public async Task<Like> GetLikeByIdAsync(int id)
        {
            return await _context.Set<Like>().FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<Like> GetLikeByUserArticle(int user,int article)
        {
            return await _context.Set<Like>().FirstOrDefaultAsync(l => l.ArticleId == article && l.UserId == user );
        }

        public async  Task AddLikeAsync(Like like)
        {
             _context.Set<Like>().Add(like);
             await _context.SaveChangesAsync();
        }

        public async Task DeleteLikeAsync(int id)
        {
            var like = await GetLikeByIdAsync(id);
            if (like != null)
            {
                _context.Set<Like>().Remove(like);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateLikeAsync(Like like)
        {
            _context.Set<Like>().Update(like);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId)
        {
            return await _context.Set<Like>().Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Like>> GetLikesByArticleIdAsync(int articleId)
        {
            return await _context.Set<Like>().Where(l => l.ArticleId == articleId).ToListAsync();
        }

        public async Task<bool> HasLikedAsync(int articleId, int userId)
        {
            // Check if the user has already liked the article
            var liked = await _context.Set<Like>()
                .AnyAsync(like => like.ArticleId == articleId &&like.UserId == userId);

            return liked;
        }
    }
}
