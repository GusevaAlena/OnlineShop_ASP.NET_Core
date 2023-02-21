using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class AddressViewModel
    {
        [Required(ErrorMessage ="Введите город")]
        [StringLength(25,MinimumLength =2,ErrorMessage ="Название города должно быть" +
            "не короче 2 символов и не длиннее 25")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите улицу")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Название улицы должно быть" +
            "не короче 2 символов и не длиннее 25")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Введите номер/корпус дома")]
  
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Введите номер парадной")]
        public int EntranceNumber { get; set; }

        [Required(ErrorMessage = "Введите номер этажа")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Введите номер квартиры")]
        public int FlatNumber { get; set; }
    }
}
