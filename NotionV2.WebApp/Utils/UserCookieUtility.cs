using System;
using Microsoft.AspNetCore.Http;

namespace NotionV2.Utils
{
    /// <summary>
    /// Утилитарный класс, предоставляющий статические методы для сохранения в куки и получения данных из кук об авторизованном пользователе.
    /// </summary>
    public static class UserCookieUtility
    {
        /// <summary>
        /// Получает логин авторизованного пользователя и значение, показывающее, что пользователь имеет права администратора.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <returns>Кортеж значений, содержащий логин авторизованного пользователя и значение, показывающее, что пользователь имеет права администратора.</returns>
        public static (string, bool) GetSavedUser(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("Site-UserName", out var login))
                return (string.Empty, false);

            if (string.IsNullOrWhiteSpace(login))
                return (string.Empty, false);

            var isAdmin = false;
            if (context.Request.Cookies.TryGetValue("Site-UserAdmin", out var isAdminValue))
                bool.TryParse(isAdminValue, out isAdmin);

            return (login, isAdmin);
        }

        /// <summary>
        /// Задает данные об авторизованном пользователе.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="isAdmin">Значение, показывающее, что пользователь имеет права администратора.</param>
        public static void SetSavedUser(HttpContext context, string login, bool isAdmin)
        {
            context.Response.Cookies.Append("Site-UserName", login, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            context.Response.Cookies.Append("Site-UserAdmin", isAdmin.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(7) });
        }
    }
}