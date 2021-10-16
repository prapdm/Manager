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


                if (!_dbContext.User.Any())
                {
                    var users = GetUsers();
                    _dbContext.User.AddRange(users);
                    _dbContext.SaveChanges();
                }


            }
        }

        private void DeleteAll()
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Users");
            _dbContext.SaveChanges();
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
                    PasswordHash = "6f3882bd3075bb465dc237f226afff18",
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
                    PasswordHash = "f20e7221a29fcb05711c4d0218825320",
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
                    PasswordHash = "54fb52e8ffa2b30cb62b337944e43ca4",
                    CreatedAt = DateTime.Now

                }
            };

            return users;

        }
    

    }
}
