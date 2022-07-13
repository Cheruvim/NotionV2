using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public IActionResult Index([FromQuery] int sectionId = -1)
        {
            var currentUser = GetUserInfoFromCookies();

            if(currentUser != null)
            {
                ViewBag.CurrentUserName = currentUser.Login;
                ViewBag.CurrentUserAdmin = currentUser.IsAdmin;
            }

            ViewBag.SelectedSection = sectionId;

            var viewModel = new HomeViewMode();

            if (currentUser == null)
                return View(viewModel);


            var sections = _db.LinkerSectionsOnUsers
                .Include(e => e.Section)
                .Include(e => e.User)
                .Where(e => e.UserId == currentUser.Id)
                .Select(e => e.Section)
                .ToList();
            viewModel.User = currentUser;
            viewModel.Sections = sections;

            viewModel.Notes = sectionId < 0 ? _db.Notes.Where(e=>sections.Contains(e.Section)).ToList() : _db.Notes.Where(e=> e.SectionId == sectionId).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SaveSection([FromForm] int sectionId, [FromForm] string sectionTitle)
        {
            var currentUser = GetUserInfoFromCookies();
            if(currentUser == null)
                return StatusCodeWithMessage(HttpStatusCode.Unauthorized, "Не удалось получить данные пользователя.");

            if (sectionId < 1)
            {
                var id = _db.Sections.Add(new Section
                {
                    Name = sectionTitle
                });

                _db.SaveChanges();
                _db.LinkerSectionsOnUsers.Add(new SectionOnUserLinker
                {
                    UserId = currentUser.Id,
                    SectionId = id.Entity.Id
                });
                _db.SaveChanges();
            }
            else
            {
                var currentSection = _db.Sections.FirstOrDefault(s => s.Id == sectionId);
                if (currentSection == null)
                    return RedirectToAction("Index");

                currentSection.Name = sectionTitle;
                _db.Sections.Update(currentSection);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SavePost([FromForm] int postId, [FromForm] int sectionId, [FromForm] string postTitle, [FromForm] string postText)
        {
            var currentUser = GetUserInfoFromCookies();
            if(currentUser == null)
                return StatusCodeWithMessage(HttpStatusCode.Unauthorized, "Не удалось получить данные пользователя.");

            if (postId < 0)
            {
                if (sectionId < 0)
                    return StatusCodeWithMessage(HttpStatusCode.Unauthorized,
                        "Не удалось сохранить пост. Не указан идентификатор категории.");

                _db.Notes.Add(new Note
                {
                    SectionId = sectionId,
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