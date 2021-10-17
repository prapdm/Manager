using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Manager.Entities;
using System.Threading.Tasks;

namespace Manager.Services
{
    public interface IAccountService
    {
        bool LoginUser(LoginUserDto dto);
    
        void RegisterUser(RegisterUserDto dto);
   
    }
    public class AccountService : IAccountService
    {
        private readonly ManagerDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountService(ManagerDbContext context, IPasswordHasher<User> passwordHasher, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.User.Add(newUser);
            _context.SaveChanges();

        }
        public  bool LoginUser(LoginUserDto dto)
        {
            var user = _context.User
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
                return false;
            

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return false;

            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                 new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                 new Claim("Email", user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                IsPersistent = true,
            };

            LoginAsync(new ClaimsPrincipal(claimsIdentity),authProperties);
            return true;

        }

        private async void LoginAsync(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authentication)
        {
            await _contextAccessor.HttpContext.SignInAsync(claimsPrincipal, authentication);
        }

    }


}
