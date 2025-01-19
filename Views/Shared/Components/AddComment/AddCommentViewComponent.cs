using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models;
using NEWS_App.Models.IRepository;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Claims;
using NEWS_App.Models.IRepositoryImpl;

namespace NEWS_App.ViewComponents
{
    public class AddCommentViewComponent : ViewComponent
    {
        private readonly ICommentRepository _commentService;
        public AddCommentViewComponent(ICommentRepository commentService)
        {
            _commentService = commentService;
        }

        public IViewComponentResult Invoke(int articleId )
        {

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var model = new Comment
            {
                ArticleId = articleId,
                UserId = userId
            };
            return View(model);
        }
    }
}
