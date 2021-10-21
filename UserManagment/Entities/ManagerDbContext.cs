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


        }

     
    }
}
