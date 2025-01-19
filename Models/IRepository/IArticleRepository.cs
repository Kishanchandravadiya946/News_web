namespace NEWS_App.Models.IRepository
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);

        Task<Article> GetArticleByArticleCategory(int category,int article);
        Task<IEnumerable<Article>> GetCategorywiseArticle(int id);
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
        Task<IEnumerable<Article>> GetPublishedArticlesAsync();
        Task<IEnumerable<Article>> GetArticlesByCategoryIdAsync(int categoryId);

        Task<IEnumerable<Article>> GetRecommendedArticlesAsync(int categoryId, int articleId, int count);

        Task<Article> GetLatestArticleAsync();

        Task<IEnumerable<Article>> SearchArticlesByTitle(string title);
    }
}
