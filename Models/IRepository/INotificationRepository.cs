namespace NEWS_App.Models.IRepository
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task NotifyAllUsersAboutNewArticleAsync(Article article);

        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);

        int CountByUserId(int userId);

        Task DeleteNotificationsByUserIdAsync(int userId);

    }

}
