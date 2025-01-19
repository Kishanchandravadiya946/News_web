using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Scripting;
using static System.Collections.Specialized.BitVector32;

namespace NEWS_App.Controllers
{
    public class LikesController : Controller
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IArticleRepository _articleRepository;
        public LikesController(ILikeRepository likeRepository, IArticleRepository articleRepository)
        {
            _likeRepository = likeRepository;
            _articleRepository = articleRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Like(int articleId)
        {
            var userId =  HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                //HttpContext.Session.SetString("ReturnUrl", currentUrl);
                return Json(new { redirectToLogin = true });
            }

            var article = await _articleRepository.GetArticleByIdAsync(articleId);

            var existingLike = await _likeRepository.GetLikeByUserArticle(userId,articleId);

            if (existingLike != null )
            {
                article.LikeCount--;
                await _likeRepository.DeleteLikeAsync(existingLike.Id); 
                await _articleRepository.UpdateArticleAsync(article);
                return Json(new { liked = false,LikeCount=article.LikeCount });
            }
            else
            {
                article.LikeCount++;
                Like like = new Like();
                like.UserId = userId;
                like.ArticleId = articleId;
                await _likeRepository.AddLikeAsync(like);
                await _articleRepository.UpdateArticleAsync(article);
                return Json(new { liked = true, LikeCount = article.LikeCount });
            }
        }







        public async Task<IActionResult> Index()
        {
            var likes = await _likeRepository.GetAllLikesAsync();
            return View(likes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var like = await _likeRepository.GetLikeByIdAsync(id);
            if (like == null) return NotFound();
            return View(like);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Like like)
        {
            if (!ModelState.IsValid) return View(like);
            await _likeRepository.AddLikeAsync(like);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var like = await _likeRepository.GetLikeByIdAsync(id);
            if (like == null) return NotFound();
            return View(like);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Like like)
        {
            if (!ModelState.IsValid) return View(like);
            await _likeRepository.UpdateLikeAsync(like);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var like = await _likeRepository.GetLikeByIdAsync(id);
            if (like == null) return NotFound();
            return View(like);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _likeRepository.DeleteLikeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> HasLiked(int articleId)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            bool hasLiked = false;
            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                //HttpContext.Session.SetString("ReturnUrl", currentUrl);
                return Json(hasLiked);
            }
             hasLiked = await _likeRepository.HasLikedAsync(articleId, userId);
            return Json(hasLiked);
        }

    }

}
