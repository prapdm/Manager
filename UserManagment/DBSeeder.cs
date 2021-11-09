using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Entities;

namespace Manager
{
    public class DBSeeder
    {
        private readonly ManagerDbContext _dbContext;

        public DBSeeder(ManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {

            if (_dbContext.Database.CanConnect())
            {
               // DeleteAll();

                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }


                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Categories.Any())
                {
                    var main_categories = GetCategories();
                    var sub_categories = GetSubCategories();
                    _dbContext.Categories.AddRange(main_categories);
                    _dbContext.SaveChanges();
                    _dbContext.Categories.AddRange(sub_categories);
                    _dbContext.SaveChanges();
                }


            }
        }

        private void DeleteAll()
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Users");
            _dbContext.SaveChanges();
        }

        private static IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>()
            {
                //Hosting Id=1
                new Category()
                {
                    Name = "Hosting",
                    Icon = "fas fa-database",
                    Slug  = "hosting",
                    IsActive   = true,
                    CreatedAt = DateTime.Now
                },
                //Emails Id=2
                new Category()
                {
                    Name = "Emails",
                    Icon = "far fa-envelope",
                    Slug  = "emails",
                    IsActive   = true,
                    CreatedAt = DateTime.Now
                },
                //Servers Id=3
                new Category()
                {
                    Name = "Servers",
                    Icon = "fas fa-server",
                    Slug  = "servers",
                    IsActive   = true,
                    CreatedAt = DateTime.Now
                },
                //Security Id = 4
                new Category()
                {
                    Name = "Security",
                    Icon = "fab fa-expeditedssl",
                    Slug  = "security",
                    IsActive   = true,
                    CreatedAt = DateTime.Now
                }, 
                //Domains Id = 5
                new Category()
                {
                    Name = "Domains",
                    Icon = "fas fa-globe-europe",
                    Slug  = "domains",
                    IsActive   = true,
                    CreatedAt = DateTime.Now
                }, 

 
               
 
            };

            return categories;
        }

        private static IEnumerable<Category> GetSubCategories()
        {
            var categories = new List<Category>()
            {
                //Hosting
                new Category()
                {
                    Name = "Enterprise Hosting",
                    Slug  = "enterprise-hosting",
                    IsActive   = true,
                    ParentId   = 1,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Business Hosting",
                    Slug  = "business-hosting",
                    IsActive   = true,
                    ParentId   = 1,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Wordpress Hosting",
                    Slug  = "wordpress-hosting",
                    IsActive   = true,
                    ParentId   = 1,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Cloud Hosting",
                    Slug  = "cloud-hosting",
                    IsActive   = true,
                    ParentId   = 1,
                    CreatedAt = DateTime.Now
                },
                //Emails Id=2
                new Category()
                {
                    Name = "Enterprise Email",
                    Slug  = "enterprise-email",
                    IsActive   = true,
                    ParentId   = 2,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Business Email",
                    Slug  = "business-email",
                    IsActive   = true,
                    ParentId   = 2,
                    CreatedAt = DateTime.Now
                },
                //Servers Id=3
                new Category()
                {
                    Name = "VPS Linux",
                    Slug  = "vps-linux",
                    IsActive   = true,
                    ParentId   = 3,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "VPS Windows",
                    Slug  = "vps-windows",
                    IsActive   = true,
                    ParentId   = 3,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Dedicated Server Linux",
                    Slug  = "dedicated-server-linux",
                    IsActive   = true,
                    ParentId   = 3,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Dedicated Server Windows",
                    Slug  = "dedicated-server-windows",
                    IsActive   = true,
                    ParentId   = 3,
                    CreatedAt = DateTime.Now
                },

                //Security Id = 4
                new Category()
                {
                    Name = "SSL",
                    Slug  = "ssl",
                    IsActive   = true,
                    ParentId   = 4,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "SiteLock",
                    Slug  = "sitelock",
                    IsActive   = true,
                    ParentId   = 4,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "CodeGuard",
                    Slug  = "codeguard",
                    IsActive   = true,
                    ParentId   = 4,
                    CreatedAt = DateTime.Now
                },
                //Domains Id = 5
                new Category()
                {
                    Name = "Premium domains",
                    Slug  = "premium-domains",
                    IsActive   = true,
                    ParentId   = 5,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Global domains",
                    Slug  = "global-domains",
                    IsActive   = true,
                    ParentId   = 5,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Europe domains",
                    Slug  = "europe-domains",
                    IsActive   = true,
                    ParentId   = 5,
                    CreatedAt = DateTime.Now
                },
                new Category()
                {
                    Name = "Donuts domains",
                    Slug  = "donuts-domains",
                    IsActive   = true,
                    ParentId   = 5,
                    CreatedAt = DateTime.Now
                },
                

            };

            return categories;
        }

        private static IEnumerable<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    Role = new Role()
                    {
                        Name = "Administrator",
                        Description = "Can view, create, update and delete customers and resellers",
                        CreatedAt = DateTime.Now
                    },
                    Name = "Jan",
                    Surname = "Admiński",
                    Email = "admin@Manager.pl",
                    Phone = "+48.654125456",
                    IsActive = true,
                    PasswordHash = "+7IvXx8bCmbADWM9JDzJYVqxdbjrz8fO/S7c51Q==",
                    CreatedAt = DateTime.Now
                },
                new User()
                {
                    Role = new Role()
                    {
                        Name = "Manager",
                        Description = "Can view, create and update customers and resellers",
                        CreatedAt = DateTime.Now
                    },
                    Name = "Stefan",
                    Surname = "Fajkowski",
                    Email = "s.fajkowski@Manager.pl",
                    Phone = "+48.634242553",
                    IsActive = false,
                    PasswordHash = "+7IvXx8bCmbADWM9JDzJYVqxdbjrz8fO/S7c51Q==",
                    CreatedAt = DateTime.Now

                },
                new User()
                {
                    Role = new Role()
                    {
                        Name = "Reseller",
                        Description = "Can view, create and update yourself and own customers",
                        CreatedAt = DateTime.Now
                    },
                    Name = "August",
                    Surname = "Wienkowski",
                    Email = "a.wienkowski@itsolutions.pl",
                    Phone = "+48.553334234",
                    IsActive = false,
                    PasswordHash = "+7IvXx8bCmbADWM9JDzJYVqxdbjrz8fO/S7c51Q==",
                    CreatedAt = DateTime.Now

                }
            };

            return users;

        }
    

    }
}
