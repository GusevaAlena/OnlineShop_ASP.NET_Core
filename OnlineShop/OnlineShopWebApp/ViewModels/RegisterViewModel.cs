using OnlineShopWebApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Для регистрации необходимо ввести e-mail")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Придумайте пароль")]
        [PasswordPropertyText(true)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{1,16}$",
            ErrorMessage = "Пароль должен состоять хотя бы из одной цифры, одной буквы, одной" +
            "заглавной буквы и одного символа, длина пароля не должна превышать 16 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [PasswordPropertyText(true)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
        public string ReturnUrl { get; set; }
    }
}
