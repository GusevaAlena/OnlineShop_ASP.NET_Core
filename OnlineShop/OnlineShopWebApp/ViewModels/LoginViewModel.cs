using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Для входа необходимо ввести ваш email")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Для входа необходимо ввести пароль")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
