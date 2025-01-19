namespace NEWS_App.Models.IRepository
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetAllLikesAsync();
        Task<Like> GetLikeByIdAsync(int  id);
        Task<Like> GetLikeByUserArticle(int user , int article);
        Task AddLikeAsync(Like like);
        Task DeleteLikeAsync(int id);

        Task UpdateLikeAsync(Like like);
        Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId);
        Task<IEnumerable<Like>> GetLikesByArticleIdAsync(int articleId);

        Task<bool> HasLikedAsync(int articleId, int userId);
    }
}
