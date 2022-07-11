using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotionV2.DataServices.Models;
using NotionV2.Models;
using NotionV2.Models.Account;
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

        public IActionResult Index()
        {
            var currentUser = GetUserInfoFromCookies();

            if(currentUser != null)
            {
                ViewBag.CurrentUserName = currentUser.Login;
                ViewBag.CurrentUserAdmin = currentUser.IsAdmin;
            }

            var viewModel = new HomeViewMode();

            if (currentUser != null)
            {
                viewModel.User = currentUser;
                viewModel.Notes = _db.Notes.Where(note => note.UserId == currentUser.Id).ToList();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SavePost([FromForm] int postId, [FromForm] string postTitle, [FromForm] string postText)
        {
            var currentUser = GetUserInfoFromCookies();
            if(currentUser == null)
                return StatusCodeWithMessage(HttpStatusCode.Unauthorized, "Не удалось получить данные пользователя.");

            if (postId < 0)
            {
                _db.Notes.Add(new Note
                {
                    Title = postTitle,
                    Body = postText,
                    UserId = currentUser.Id
                });

                _db.SaveChanges();
            }
            else
            {
                var currentNote = _db.Notes.FirstOrDefault(note => note.Id == postId);
                if (currentNote == null)
                    return RedirectToAction("Index");

                currentNote.Body = postText;
                currentNote.Title = postTitle;
                _db.Notes.Update(currentNote);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeletePost([FromQuery] int postId)
        {
            var currentNote = _db.Notes.FirstOrDefault(note => note.Id == postId);
            var currentUser = GetUserInfoFromCookies();
            if(currentUser == null)
                return StatusCodeWithMessage(HttpStatusCode.Unauthorized, "Не удалось получить данные пользователя.");

            if (currentNote == null)
                return StatusCodeWithMessage(HttpStatusCode.BadRequest, "Не удалось найти запись с данным идентификатором.");

            if (currentNote.UserId != currentUser.Id)
                return StatusCodeWithMessage(HttpStatusCode.BadRequest, "Вы не создатель поста!");

            _db.Remove(currentNote);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Возвращает контент с сообщением и заданным кодом.
        /// </summary>
        /// <param name="code">Код статуса ответа на HTTP запрос.</param>
        /// <param name="message">Сообщение.</param>
        /// <returns>Результат выполнения запроса.</returns>
        private IActionResult StatusCodeWithMessage(HttpStatusCode code, string message)
        {
            var contentResult = Content($"{code}{Environment.NewLine}{message}");
            contentResult.StatusCode = (int)code;
            return contentResult;
        }

        private AccountViewModel GetUserInfoFromCookies()
        {
            var (userName, _) = UserCookieUtility.GetSavedUser(HttpContext);

            if (userName == null)
                return null;

            var currentUser = _db.Users.FirstOrDefault(user => user.Name == userName);

            if (currentUser == null)
                return null;

            var result = new AccountViewModel
                { Id = currentUser.Id, Login = currentUser.Name, IsAdmin = currentUser.IsAdmin };
            return result;
        }
    }
}