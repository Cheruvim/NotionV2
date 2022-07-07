using System.ComponentModel.DataAnnotations;

namespace NotionV2.Models.Account
{
    /// <summary>
    /// Модель отображения учетных данных пользователя на странице авторизации.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Получает или задает логин пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Логин пользователя")]
        public string Login { get; set; }

        /// <summary>
        /// Получает или задает пароль пользователя.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль пользователя")]
        public string Password { get; set; }
    }
}