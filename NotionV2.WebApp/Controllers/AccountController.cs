using System;
using System.Diagnostics;
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
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public AccountController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _db = context;
        }
        
        /// <summary>
        /// Обрабатывает запрос на переход к главной странице контроллера управления аккаунтами.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            // Выполняет переадресацию на страницу авторизации (метод Login() контроллера управления аккаунтами).
            return RedirectToAction("Login");
        }
        
        /// <summary>
        /// Обрабатывает запрос на переход к странице авторизации.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            // Получает из кук авторизованного пользователя. Если пользователь найден,
            // выполняет переадресацию на главную страницу приложения (метод Index контроллера главной страницы).
            // Иначе возвращает страницу авторизации.
            var (userName, _) = UserCookieUtility.GetSavedUser(HttpContext);
            if (!string.IsNullOrWhiteSpace(userName))
                return RedirectToAction("Index", "Home");

            return View();
        }
        
        /// <summary>
        /// Обрабатывает запрос на авторизацию.
        /// </summary>
        /// <param name="model">Модель отображения учетных данных пользователя на странице авторизации.</param>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Вызывает проверку соответствия логина и пароля введенных пользователем с сохраненными в БД.
            // Если проверка не пройдена, возвращает страницу ошибки авторизации.
            try
            {
                var isPasswordCorrect = IsPasswordCorrect(model.Login, model.Password);
                if (!isPasswordCorrect)
                    return StatusCodeWithMessage(HttpStatusCode.Unauthorized, "Неверно указан логин или пароль");
            }
            catch (Exception e)
            {
                return StatusCodeWithMessage(HttpStatusCode.Unauthorized, e.Message);
            }

            // Сохраняет авторизованного пользователя в куки и выполняет переадресацию
            // на главную страницу приложения (метод Index контроллера главной страницы).
            var userModel = _db.Users.Where(user=> user.Name == model.Login).FirstOrDefault();
            UserCookieUtility.SetSavedUser(HttpContext, userModel.Name, userModel.IsAdmin);

            return RedirectToAction("Index", "Home");
        }

        private bool IsPasswordCorrect(string login, string password)
        {
            // Запрашивается пользователь по логину.
            var existingUser = _db.Users.Where(user => user.Name == login && user.Password == password);

            if (existingUser.FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Открывает страницу регистрации пользователя.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            // Получает из кук авторизованного пользователя. Если пользователь найден,
            // выполняет переадресацию на главную страницу приложения (метод Index контроллера главной страницы).
            // Иначе возвращает страницу регистрации.
            var (userName, _) = UserCookieUtility.GetSavedUser(HttpContext);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Обрабатывает запрос на регистрацию пользователя.
        /// </summary>
        /// <param name="model">Модель отображения учетных данных пользователя на странице регистрации пользователя.</param>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                // Вызывает добавление пользователя в БД и выполняет переадресацию
                // на страницу авторизации (метод Login контроллера управления аккаунтами)
                var usersWithLogin = _db.Users.Where(user => user.Name == model.Login).FirstOrDefault();
                if(usersWithLogin != null)
                    return StatusCodeWithMessage(HttpStatusCode.BadRequest, "Пользователь с данным логином уже существует.");

                if(string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                    return StatusCodeWithMessage(HttpStatusCode.BadRequest, "Заполните поля с логином и паролем.");

                _db.Users.Add(new User
                {
                    Name = model.Login,
                    Password = model.Password
                });
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return StatusCodeWithMessage(HttpStatusCode.BadRequest, e.Message);
            }

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Обрабатывает запрос на деавторизацию.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Logout()
        {
            // Сохраняет в куки пустые данные о текущем пользователе и вызывает переадресацию
            // на страницу авторизации (метод Login контроллера управления аккаунтами).
            UserCookieUtility.SetSavedUser(HttpContext, string.Empty, false);
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Возвращает результат выполнения запроса с данными об ошибке.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Возвращает страницу ошибки.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}