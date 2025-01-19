using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Models.IRepositoryImpl
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context; // Replace with your actual DbContext class

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Set<Notification>().AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task NotifyAllUsersAboutNewArticleAsync(Article article)
        {
            var users = await _context.Set<User>().Where(u => u.notification == true).ToListAsync(); // Get all users
            foreach (var user in users)
            {
                var notification = new Notification
                {
                    UserId = user.Id,
                    ArticleId = article.Id,
                    CreatedAt = DateTime.Now
                };
                await AddNotificationAsync(notification); // Call the AddNotificationAsync method to save
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Set<Notification>().OrderByDescending(a => a.CreatedAt)
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public int CountByUserId(int userId)
        {
            return _context.Set<Notification>()
                .Where(n => n.UserId == userId)
                .Count();
        }

        public async Task DeleteNotificationsByUserIdAsync(int userId)
        {
            var notifications = _context.Set<Notification>()
                .Where(n => n.UserId == userId);

            _context.Set<Notification>().RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }
    }

}
