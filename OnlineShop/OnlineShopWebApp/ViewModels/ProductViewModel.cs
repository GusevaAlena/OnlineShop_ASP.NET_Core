using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите название модели")]
        [StringLength(50,ErrorMessage ="Слишком длинное название модели")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Укажите цену товара")]
        [Range(20,500000,ErrorMessage ="Цена должна быть в диапазоне от 20 до 500000")]
        [RegularExpression("^[0-9]+$",ErrorMessage ="Цена должна быть целым числом")]
        public decimal Price { get; set; }

        [Required(ErrorMessage ="Необходимо коротко описать товар")]
        [StringLength(800,MinimumLength =20,ErrorMessage ="Описание товара должно быть" +
            " от 20 до 800 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Укажите общее количество товара на складе")]
        [Range(0,50,ErrorMessage ="Количество товара на складе не может быть больше 50 ед.")]
        public int TotalQuantity { get; set; } 
        public IFormFile UploadedFile { get; set; }
        public string ImagePath { get; set; }
    }
}
