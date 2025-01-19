namespace NEWS_App.Models.IRepository
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int id);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId,string sort);
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    }
}
