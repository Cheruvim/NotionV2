using System.ComponentModel.DataAnnotations;

namespace NotionV2.Models.Account
{
    /// <summary>
    /// Модель отображения учетных данных пользователя на странице регистрации пользователя.
    /// </summary>
    public class RegisterViewModel
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

        /// <summary>
        /// Получает или задает пароль пользователя, используемый для проверки правильности ввода.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}