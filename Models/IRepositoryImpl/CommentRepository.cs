using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Models.IRepositoryImpl
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Set<Comment>().ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Set<Comment>().FindAsync(id);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Set<Comment>().AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Set<Comment>().Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await GetCommentByIdAsync(id);
            if (comment != null)
            {
                _context.Set<Comment>().Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId, string sort)
        {
            var query = _context.Set<Comment>().Where(c => c.ArticleId == articleId);

            switch (sort?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(c => c.CreatedDate);
                    break;
                case "oldest":
                    query = query.OrderBy(c => c.CreatedDate);
                    break;
                default:
                    query = query.OrderByDescending(c => c.CreatedDate);
                    break;
            }

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await _context.Set<Comment>().Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
