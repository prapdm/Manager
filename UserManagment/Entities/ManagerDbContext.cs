using Microsoft.EntityFrameworkCore;

/*
Get-Migrations
add-migration name_of_migration
remove-migration 
update-database
*/

namespace Manager.Entities
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Service> Services { get; set; } 
        public DbSet<Price> Prices { get; set; } 

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Users 
            modelBuilder.Entity<User>()
               .Property(u => u.Name)
               .HasMaxLength(32);

            modelBuilder.Entity<User>()
               .Property(u => u.Surname)
               .HasMaxLength(150);

            modelBuilder.Entity<User>()
               .Property(u => u.Email)
               .HasMaxLength(150)
               .IsRequired();

            modelBuilder.Entity<User>()
               .Property(u => u.Phone)
               .HasMaxLength(20);

            modelBuilder.Entity<User>()
              .Property(u => u.IsActive)
              .HasDefaultValue(false);

            modelBuilder.Entity<User>()
              .Property(u => u.PasswordHash)
              .IsRequired();

            modelBuilder.Entity<User>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAdd()
              .IsRequired();

            modelBuilder.Entity<User>()
              .Property(u => u.UpdatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAddOrUpdate()
              .IsRequired();

            //Role
            modelBuilder.Entity<Role>()
              .Property(r => r.Name)
              .HasMaxLength(100)
              .IsRequired();

            modelBuilder.Entity<Role>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAdd()
              .IsRequired();

            modelBuilder.Entity<Role>()
              .Property(u => u.UpdatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAddOrUpdate()
              .IsRequired();

            //Categories
            modelBuilder.Entity<Category>()
            .HasOne(s => s.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Category>()
              .Property(r => r.Name)
              .HasMaxLength(100)
              .IsRequired();

            modelBuilder.Entity<Category>()
              .Property(u => u.IsActive)
              .HasDefaultValue(false);

            modelBuilder.Entity<Category>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAdd()
              .IsRequired();

            modelBuilder.Entity<Category>()
              .Property(u => u.UpdatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAddOrUpdate()
              .IsRequired();

            //Services
            modelBuilder.Entity<Service>()
               .Property(u => u.Name)
               .HasMaxLength(150)
               .IsRequired();

            modelBuilder.Entity<Service>()
               .Property(u => u.Slug)
               .IsRequired();
            
            modelBuilder.Entity<Service>()
               .Property(u => u.IsActive)
               .HasDefaultValue(false);

            modelBuilder.Entity<Service>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAdd()
              .IsRequired();

            modelBuilder.Entity<Service>()
              .Property(u => u.UpdatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAddOrUpdate()
              .IsRequired();

            //Prices
            modelBuilder.Entity<Price>()
              .Property(u => u.PurchasePrice)
              .HasColumnType("decimal(10,4)")
              .IsRequired();
            
            modelBuilder.Entity<Price>()
              .Property(u => u.SellPrice)
              .HasColumnType("decimal(10,4)")
              .IsRequired();
            
            modelBuilder.Entity<Price>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAdd()
              .IsRequired();

            modelBuilder.Entity<Price>()
              .Property(u => u.UpdatedAt)
              .HasDefaultValueSql("GETDATE()")
              .ValueGeneratedOnAddOrUpdate()
              .IsRequired();


        }


    }
}
