using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public class DatabaseContext:IdentityDbContext<User,Role,Guid>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartPosition> CartPositions { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<CompareProduct> CompareProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasData(
                    new Product ("07d8f9de-148d-46ac-8cb5-2da4407164db", "Укулеле Flight TUC Dasha Kirpich", 4600, "Первая официальная акулеле", 5, @"/images/Products/Flight-TUC-DASHA-KIRPICH_Soprano-Travel-Ukulele-1.jpg"),
                    new Product("e67cdaed-5da3-4923-aa89-a606b3ea52c5","Гитара Классика ECO CS-5", 4600, "Красивая и недорогая гитара для вас", 1, @"/images/Products/467637-gitara-klassicheskaya-eko-cs-5violet-36-78d86f.jpg"),
                    new Product("e37d60c3-e035-4080-a5f9-4421546c938c", "Гитара YAMAHA F310", 19900, "Нестареющая классика", 3, @"/images/Products/Yamaha.jpg"),
                    new Product("ad0e133b-c9c5-4ae8-a948-0a0666e584d8", "Скрипка CREMONA 15W 1/2", 30960, "Качественный выбор всех профессионалов", 1, @"/images/Products/Skripki_Cremona_15w_001_b.jpg"),
                    new Product("6ab10fa1-5127-4b84-827b-94838989b9aa", "Бас-гитара Homage HED760SB", 12660, "Недорогая бас-гитара для новичков", 2, @"/images/Products/bass-guitar.jpeg"),
                    new Product("7da2d69a-1a51-4815-9b2d-dd4463962efe", "Укулеле Victoria Tenor CE", 24050, "Принцесса Виктория из коллекции Flight Princess", 2, @"/images/Products/Flight-Soundwave-Tenor-Main.png")
                    );
                
                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.Price)
                .IsRequired();
                entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(800);
                entity.Property(e => e.TotalQuantity)
                .IsRequired();
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasData(
                     new Payment { Id = 1, Method = "Картой на сайте" },
                     new Payment { Id = 2, Method = "Картой курьеру" },
                     new Payment { Id = 3, Method = "Наличными курьеру" }
                    );
            });
            
            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasData(
                    new Status { Id = 1, Name = "Создан" },
                    new Status { Id = 2, Name = "Собран" },
                    new Status { Id = 3, Name = "Передан в доставку" },
                    new Status { Id = 4, Name = "Доставлен" },
                    new Status { Id = 5, Name = "Отменен" }
                    );
            });
        }
    }
}

