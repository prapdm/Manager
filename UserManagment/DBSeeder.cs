using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

                if (!_dbContext.Services.Any())
                {
                    var services = GetServices();
                    _dbContext.Services.AddRange(services);
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
        private static IEnumerable<Service> GetServices()
        {
            var services = new List<Service>()
            {
                new Service()
                {
                    Name = "W50 Plan",
                    Description = "Capacity: 30GB, 1 free .pl domains, Let's Encrypt certificate for free",
                    Slug = "w50-plan",
                    Tags = "Wordpress Hosting",
                    SKU = "W50",
                    IsActive = true,
                    CategoryId = 19,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 20,
                        SKU   = "W50",
                    }

                },
                new Service()
                {
                    Name = "W100 Business Plan",
                    Description = "Capacity: 100GB, 3 free .pl domains, Let's Encrypt certificate for free",
                    Slug = "w100-plan",
                    Tags = "Business Hosting",
                    SKU = "W100",
                    IsActive = true,
                    CategoryId = 18,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 50,
                        SKU   = "W100",
                    }

                },new Service()
                {
                    Name = "W200 Cloud",
                    Description = "Capacity: 200GB, 5 free .pl domains, Let's Encrypt certificate for free",
                    Slug = "w200-plan",
                    Tags = "Cloud Hosting",
                    SKU = "W200",
                    IsActive = true,
                    CategoryId = 20,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 80,
                        SKU   = "W200",
                    }

                },new Service()
                {
                    Name = "E100 Enterprise Email",
                    Description = "Email service, 50 accounts, Capacity: 100GB",
                    Slug = "e100-ent",
                    Tags = "Enterprise Email",
                    SKU = "e100-ent",
                    IsActive = true,
                    CategoryId = 6,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 55,
                        SKU   = "e100-ent",
                    }

                },new Service()
                {
                    Name = "E200 Business Email",
                    Description = "Email service for Business, unlimited accounts, Capacity: 200GB",
                    Slug = "e200",
                    Tags = "Business Email",
                    SKU = "e200",
                    IsActive = true,
                    CategoryId = 16,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 55,
                        SKU   = "e200",
                    }

                },new Service()
                {
                    Name = "E3-SAT-1-16",
                    Description = "Intel  Xeon E3-1225v2, 4c / 4t, 3.2GHz, 16GB DDR3 1333 MHz, SoftRaid  3x2TB   SATA",
                    Slug = "e3-sat-1-16",
                    Tags = "Dedicated Server Windows",
                    SKU = "E3-SAT-1-16",
                    IsActive = true,
                    CategoryId = 12,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 255,
                        SKU   = "E3-SAT-1-16",
                    }

                },new Service()
                {
                    Name = "E3-SAT-1-32",
                    Description = "Intel  Xeon E3-1245v2, 4c / 4t, 3.4GHz, 32GB DDR3 1333 MHz, SoftRaid  3x2TB SATA",
                    Slug = "e3-sat-1-32",
                    Tags = "Dedicated Server Windows",
                    SKU = "E3-SAT-1-32",
                    IsActive = true,
                    CategoryId = 12,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 275,
                        SKU   = "E3-SAT-1-32",
                    }

                },new Service()
                {
                    Name = "E3-SSD-1-32",
                    Description = "Intel  Xeon E3-1245v2, 4c / 4t, 3.4GHz, 32GB DDR3 1333 MHz, SoftRaid  3x2TB SATA",
                    Slug = "e3-ssd-1-32",
                    Tags = "Dedicated Server Linux",
                    SKU = "E3-SSD-1-32",
                    IsActive = true,
                    CategoryId = 14,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 235,
                        SKU   = "E3-SSD-1-32",
                    }

                },new Service()
                {
                    Name = "E3-SSD-2-32",
                    Description = "Intel  Xeon E3-1231v3, 4c / 8t, 3.4GHz, 32GB DDR3 1333 MHz, SoftRaid  3x480GB SSD",
                    Slug = "e3-ssd-2-32",
                    Tags = "Dedicated Server Linux",
                    SKU = "E3-SSD-2-32",
                    IsActive = true,
                    CategoryId = 14,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 330,
                        SKU   = "E3-SSD-1-32",
                    }

                },new Service()
                {
                    Name = "VALUE VPS",
                    Description = "1 vCore, 2GB, 40GB SSD NVMe, 250 Mbps unlimited",
                    Slug = "value-vps",
                    Tags = "VPS Linux",
                    SKU = "value-vps",
                    IsActive = true,
                    CategoryId = 15,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 26,
                        SKU   = "value-vps",
                    }

                },new Service()
                {
                    Name = "ESSENTIAL VPS",
                    Description = "2 vCore, 4GB, 80GB SSD NVMe, 500 Mbps unlimited",
                    Slug = "essential-vps",
                    Tags = "VPS Linux",
                    SKU = "essential-vps",
                    IsActive = true,
                    CategoryId = 15,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 35,
                        SKU   = "essential-vps",
                    }

                },new Service()
                {
                    Name = "COMFORT VPS",
                    Description = "4 vCore, 8GB, 160GB SSD NVMe, 1 Gbps unlimited",
                    Slug = "comfort-vps",
                    Tags = "VPS Windows",
                    SKU = "comfort-vps",
                    IsActive = true,
                    CategoryId = 21,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 46,
                        SKU   = "comfort-vps",
                    }

                },new Service()
                {
                    Name = "ELITE VPS",
                    Description = "8 vCore, 32GB, 500GB SSD NVMe, 2 Gbps unlimited",
                    Slug = "elite-vps",
                    Tags = "ELITE Windows",
                    SKU = "elite-vps",
                    IsActive = true,
                    CategoryId = 21,
                    Price = new Price()
                    {
                        PurchasePrice = 0,
                        SellPrice  = 150,
                        SKU   = "elite-vps",
                    }

                }



            };

            return services;

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
                        Description = "Can view, create, update",
                        CreatedAt = DateTime.Now
                    },
                    Name = "Jan",
                    Surname = "Admiński",
                    Email = "admin@manager.pl",
                    Phone = "+48.654125456",
                    IsActive = true,
                    PasswordHash = "AQAAAAEAACcQAAAAENyMVSa5mYU1Z0cd8MW9RIMeGDDWvGWNW6wz+2VEBGiHfqnvGcGGaeeymXEWwJbAvg==",
                    CreatedAt = DateTime.Now
                },
                new User()
                {
                    Role = new Role()
                    {
                        Name = "Manager",
                        Description = "Can view  and update",
                        CreatedAt = DateTime.Now
                    },
                    Name = "Stefan",
                    Surname = "Fajkowski",
                    Email = "manager@manager.pl",
                    Phone = "+48.634242553",
                    IsActive = true,
                    PasswordHash = "AQAAAAEAACcQAAAAENyMVSa5mYU1Z0cd8MW9RIMeGDDWvGWNW6wz+2VEBGiHfqnvGcGGaeeymXEWwJbAvg==",
                    CreatedAt = DateTime.Now

                },
                new User()
                {
                    Role = new Role()
                    {
                        Name = "User",
                        Description = "Can view",
                        CreatedAt = DateTime.Now
                    },
                    Name = "August",
                    Surname = "Wienkowski",
                    Email = "user@manager.pl",
                    Phone = "+48.553334234",
                    IsActive = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEApugCGEqb6o7UxWAZkMa5nzUgVEJZCVheEEyofMZ7/AVucqyq1KjjqQ1/CReYjtcg==",
                    CreatedAt = DateTime.Now

                }
            };

            return users;

        }
    

    }
}
