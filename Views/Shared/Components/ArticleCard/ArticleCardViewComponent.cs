using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models;
using NEWS_App.Models.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NEWS_App.Components
{
    public class ArticleCardViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleCardViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // InvokeAsync allows you to pass parameters like categoryId and count if needed
        public async Task<IViewComponentResult> InvokeAsync(int catrgoryId,int articleId)
        {
            int count = 5;
            var Article = await _articleRepository.GetRecommendedArticlesAsync(catrgoryId,articleId, count);
            return View(Article);
        }
    }
}
