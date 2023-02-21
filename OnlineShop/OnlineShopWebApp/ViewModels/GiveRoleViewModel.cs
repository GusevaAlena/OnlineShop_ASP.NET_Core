using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db.Models;
using System.Collections.Generic;

namespace OnlineShopWebApp.ViewModels
{
    public class GiveRoleViewModel
    {
        public string UserName { get;set; }
        public List<RoleViewModel> UserRoles { get;set; }
        public List<RoleViewModel> AllRoles { get; set; }
    }
}
