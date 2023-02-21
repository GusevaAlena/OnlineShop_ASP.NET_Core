using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Введите Ваше имя, чтобы мы знали как к Вам обращаться")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Вашу фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона для связи")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$",
            ErrorMessage = "Неверный формат ввода номера телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Укажите Ваш email, на него мы отправим всю информацию о заказе")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        public string Email { get; set; }
    }
}
