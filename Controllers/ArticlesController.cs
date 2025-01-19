using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Http;

namespace NEWS_App.Controllers
{
    
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationRepository _notiRepository;

        public ArticlesController(IArticleRepository articleRepository,ICategoryRepository categoryRepository, INotificationRepository notiRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _notiRepository = notiRepository;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> category(string name)
        {
          
            var category =await _categoryRepository.GetCategoryIdByNameAsync(name);
            if(category == null) return NotFound("not found  given category" + name);

            int id=category.Id;
            var articles= await _articleRepository.GetCategorywiseArticle(id);

            ViewBag.CategoryName = name;
            return View(articles);
        }

        public async Task<IActionResult> Unique(int category,int article)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            HttpContext.Session.SetInt32("categoryId", category);
            HttpContext.Session.SetInt32("articleId", article);
            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return RedirectToAction("Login","User");
            }

            var UniqueArticle = await _articleRepository.GetArticleByArticleCategory(category,article);
            if (UniqueArticle == null) return NotFound("not found  that Article");
            return View(UniqueArticle);
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.Session.GetString("Username") ?? "temp";
            if(userId != "admin@gmail.com") return NotFound();
            var category = await _categoryRepository.GetAllCategoriesAsync();
            if (category == null) return NotFound();
            else
            {
                ViewBag.Category = new SelectList(category, "Id", "Name");
            }
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article, IFormFile imageFile, IFormFile videoFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                article.ImageUrl = "/images/" + imageFile.FileName;
            }
            else
            {
                article.ImageUrl = "No Images";
            }
            if (videoFile != null && videoFile.Length > 0)
            {
                var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", videoFile.FileName);
                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }
                article.VideoUrl = "/videos/" + videoFile.FileName;
            }
            else
            {
                article.VideoUrl = "No Videos";
            }
            article.PublishedDate = DateTime.Now;
            article.LikeCount = 0;
            await _articleRepository.AddArticleAsync(article);

            var newArticle = await _articleRepository.GetLatestArticleAsync();

            await _notiRepository.NotifyAllUsersAboutNewArticleAsync(newArticle);
            return RedirectToAction("Index", "Articles");
        }


        public async Task<IActionResult> Search(String title)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return RedirectToAction("Login", "User");
            }

            var articles= await _articleRepository.SearchArticlesByTitle(title);
            if (articles == null)
            {
                return NotFound("Not match your search with title .");
            }
            return View(articles);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            if (!ModelState.IsValid) return View(article);
            await _articleRepository.UpdateArticleAsync(article);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _articleRepository.DeleteArticleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
