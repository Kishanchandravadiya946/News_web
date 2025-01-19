using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS_App.Models;
using NEWS_App.Models.IRepository;

namespace NEWS_App.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IArticleRepository _articleRepository;
        public NotificationController(INotificationRepository notificationRepository, IArticleRepository articleRepository)
        {
            _notificationRepository = notificationRepository;
            _articleRepository = articleRepository;
        }


        List<Article> articlesList = new List<Article>();

        public async Task<ActionResult> Artical()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
            {
                return NotFound("signup please");
            }

            List<Article> articlesList = new List<Article>();

            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            foreach (var notification in notifications)
            {
                var article = await _articleRepository.GetArticleByIdAsync(notification.ArticleId);

                if (article != null)
                {
                    articlesList.Add(article);
                }
            }

            HttpContext.Session.Remove("Notification");

            //_ = Task.Run(async () =>
            //{
               // await Task.Delay(TimeSpan.FromMinutes(5)); // Wait for 30 minutes

                await _notificationRepository.DeleteNotificationsByUserIdAsync(userId);
            //});
            return View(articlesList);
        }
    }
}
