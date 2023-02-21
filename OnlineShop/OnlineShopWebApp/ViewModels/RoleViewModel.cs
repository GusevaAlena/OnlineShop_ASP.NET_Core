using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Введите название роли")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Название роли должно быть " +
            "не короче 3 и не длиннее 25 символов")]
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            var role = obj as RoleViewModel;
            return Name==role.Name;
        }
    }
}
