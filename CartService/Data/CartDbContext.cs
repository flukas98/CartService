using Microsoft.EntityFrameworkCore;
using CartService.Data.Model;
using CartService.Data.Model.ProductCatalogServiceModel;
using CartService.Data.Model.IdentityServiceModel;
using CartService.Data.Model.InventoryServiceModel;

namespace CartService.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Racket> Rackets { get; set; } = null!;
        public DbSet<Ball> Balls { get; set; } = null!;
        public DbSet<Shoes> Shoes { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<ItemQuantity> ItemQuantities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Username and Email must each be unique across users.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // One cart per user: enforced at the DB level so a race between two
            // concurrent requests can't create duplicate carts for the same user.
            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            // Cart -> User: explicit FK (User has no inverse Carts collection).
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configure composite key for join entity
            modelBuilder.Entity<CartItem>().HasKey(ci => new { ci.CartId, ci.ItemId });

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Item)
                .WithMany()
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table-per-type: each concrete Item subclass gets its own table
            // (Rackets/Balls/Shoes), instead of one shared Items table with a
            // discriminator column. Item itself keeps the shared base columns.
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Racket>().ToTable("Rackets");
            modelBuilder.Entity<Ball>().ToTable("Balls");
            modelBuilder.Entity<Shoes>().ToTable("Shoes");

            // One quantity record per item: enforced at the DB level.
            modelBuilder.Entity<ItemQuantity>()
                .HasIndex(iq => iq.ItemId)
                .IsUnique();

            modelBuilder.Entity<ItemQuantity>()
                .HasOne<Item>()
                .WithMany()
                .HasForeignKey(iq => iq.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data: 2 of each item type. HasData needs fixed, hardcoded keys
            // (not Guid.NewGuid()) so the seed is deterministic across migrations.
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111101"),
                    Username = "flukas",
                    Email = "flukas@example.com",
                    FirstName = "Filip",
                    LastName = "Lukas"
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222201"),
                    Username = "aantic",
                    Email = "aantic@example.com",
                    FirstName = "Ante",
                    LastName = "Antic"
                });
            
            modelBuilder.Entity<Racket>().HasData(
                new Racket
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111101"),
                    Name = "Pro Racket",
                    Price = 149.99m,
                    Description = "Professional tennis racket",
                    GripSize = "L2",
                    HeadSize = 645
                },
                new Racket
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111102"),
                    Name = "Junior Racket",
                    Price = 59.99m,
                    Description = "Lightweight racket for juniors",
                    GripSize = "L0",
                    HeadSize = 600
                });

            modelBuilder.Entity<Ball>().HasData(
                new Ball
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222201"),
                    Name = "Championship Tennis Ball",
                    Price = 2.99m,
                    Description = "Regulation tennis ball",
                    Material = "Rubber/Felt",
                    Diameter = 6.7
                },
                new Ball
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222202"),
                    Name = "Practice Tennis Ball",
                    Price = 1.99m,
                    Description = "Practice-grade tennis ball",
                    Material = "Rubber/Felt",
                    Diameter = 6.7
                });

            modelBuilder.Entity<Shoes>().HasData(
                new Shoes
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333301"),
                    Name = "Court Runner",
                    Price = 89.99m,
                    Description = "Tennis court shoes",
                    Size = 42,
                    Brand = "SportX"
                },
                new Shoes
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333302"),
                    Name = "Grip Master",
                    Price = 99.99m,
                    Description = "High-grip tennis shoes",
                    Size = 43,
                    Brand = "SportX"
                });
        }
    }
}
