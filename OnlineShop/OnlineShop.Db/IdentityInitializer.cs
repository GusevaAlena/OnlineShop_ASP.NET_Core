using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var adminPassword = "!Admin123";
            var adminRoleName = "Администратор";
            var userRoleName = "Пользователь";
            if (roleManager.FindByNameAsync(adminRoleName).Result == null)
            {
                roleManager.CreateAsync(new Role { Name = adminRoleName }).Wait();
            }
            if (roleManager.FindByNameAsync(userRoleName).Result == null)
            {
                roleManager.CreateAsync(new Role { Name = userRoleName }).Wait();
            }
            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = userManager.CreateAsync(admin, adminPassword).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, adminRoleName).Wait();
                }
            }
        }
    }
}
