using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotionV2.DataServices.Models;
using NotionV2.Models;
using NotionV2.Utils;

namespace NotionV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _db = context;
        }

        public IActionResult Index([FromQuery] long category = -1, [FromQuery] long post = -1, [FromQuery] string searchText = "")
        {
            var (userName, isAdmin) = UserCookieUtility.GetSavedUser(HttpContext);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                ViewBag.CurrentUserName = userName;
                ViewBag.CurrentUserAdmin = isAdmin;
            }

            var currentUser = _db.Users.FirstOrDefault(user => user.Name == userName);
            var viewModel = new HomeViewMode();
            viewModel.User = currentUser;

            if (currentUser != null)
                viewModel.Notes = _db.Notes.Where(note => note.UserId == currentUser.Id).ToList();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SavePost([FromForm] int postId, [FromForm] string postTitle, [FromForm] string postText)
        {
            var (userName, isAdmin) = UserCookieUtility.GetSavedUser(HttpContext);
            var currentUser = _db.Users.FirstOrDefault(user => user.Name == userName);

            if (postId < 0)
            {
                _db.Notes.Add(new Note
                {
                    Title = postTitle,
                    Body = postText,
                    UserId = currentUser.Id
                });
                _db.SaveChanges();

            }else
            {
                var currentNote = _db.Notes.FirstOrDefault(note => note.Id == postId);
                if (currentNote != null)
                {
                    currentNote.Body = postText;
                    currentNote.Title = postTitle;
                    _db.Notes.Update(currentNote);
                    _db.SaveChanges();
                } 
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeletePost([FromQuery] int postId)
        {
            _db.Remove(_db.Notes.FirstOrDefault(note => note.Id == postId));
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}