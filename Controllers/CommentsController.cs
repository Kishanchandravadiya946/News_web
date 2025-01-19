using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using System.ComponentModel;

namespace NEWS_App.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> AddComment(Comment model)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                //HttpContext.Session.SetString("ReturnUrl", currentUrl);
                return RedirectToAction("Login", "User");
            }
            var comment = new Comment
            {
                ArticleId = model.ArticleId,
                UserId = userId,
                Content = model.Content,
                CreatedDate = DateTime.Now,
            };
            await _commentRepository.AddCommentAsync(comment);

            var currentCategory = HttpContext.Session.GetInt32("categoryId");
            var currentArticle = HttpContext.Session.GetInt32("articleId");
            HttpContext.Session.Remove("categoryId");
            HttpContext.Session.Remove("articleId");

            return RedirectToAction("Unique", "Articles", new { category = currentCategory, article = currentArticle });
        
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return View(comments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (!ModelState.IsValid) return View(comment);
            await _commentRepository.AddCommentAsync(comment);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (!ModelState.IsValid) return View(comment);
            await _commentRepository.UpdateCommentAsync(comment);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Comments(int articleId, string sort = "newest")
        {
            var comments = await _commentRepository.GetCommentsByArticleIdAsync(articleId, sort);
            return ViewComponent("Comments", new { articleId = articleId,sort=sort });
        }

    }

}
