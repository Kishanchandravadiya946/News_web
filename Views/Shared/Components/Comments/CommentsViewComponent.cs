using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models;
using NEWS_App.Models.IRepository; 

namespace NEWS_App.ViewComponents
{
    public class CommentsViewComponent : ViewComponent
    {
        private readonly ICommentRepository _commentService; // Assuming you have a service to handle comments

        public CommentsViewComponent(ICommentRepository commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int articleId,string sort="newest")
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId,sort);
            return View(comments);
        }
    }
}
