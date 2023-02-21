using Microsoft.AspNetCore.Http;
using OnlineShop.Db.Models;
using System.Collections.Generic;

namespace OnlineShopWebApp.ViewModels
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImagePath { get; set; }
        public IFormFile UploadedFile { get; set; }
        public List<Order> Orders { get; set; }
    }
}
