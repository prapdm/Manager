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
using FluentEmail.Core;
using System.Reflection;


namespace Manager.Services
{
    public interface IAccountService
    {
        bool VerifyPassword(LoginUserDto dto);
        void RegisterUser(RegisterUserDto dto);
        bool VeryfiyEmail(VeryfiyEmailDto dto);
   
    }
    public class AccountService : IAccountService
    {
        private readonly ManagerDbContext _context;
        private readonly IPasswordHasher<UserService> _passwordHasher;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly MailSettings _mailSettings;
        private readonly IFluentEmail _email;

        public AccountService(ManagerDbContext context, IPasswordHasher<UserService> passwordHasher, IHttpContextAccessor contextAccessor,
                MailSettings mailSettings, IFluentEmail email  )
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
            _mailSettings = mailSettings;
            _email = email;
        }
        public bool VeryfiyEmail(VeryfiyEmailDto dto)
        {
            var user = _context.Users
                .Where(u => u.Email == dto.Email)
                .Where(c => c.VerifcationCode == dto.VerifcationCode)
                .Where(c => c.IsActive == false)
                .FirstOrDefault();

            if (user is null)
                 return false;


            user.IsActive = true;
            _context.SaveChanges();
            return true;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            Random rnd = new();
            dto.VerifcationCode = rnd.Next(10000, 99999);
            var newUser = new UserService()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                RoleId = dto.RoleId,
                VerifcationCode = dto.VerifcationCode
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            var test = this.GetType().GetTypeInfo().Assembly;

            var email = _email
                .To(dto.Email)
                .BCC(_mailSettings.AdminBCC)
                .Subject("Confirm your addres email")
                .UsingTemplateFromFile(@"Views\Mail\Welcome.cshtml", dto);
 
            Task.Run(async () => await email.SendAsync());

        }

        public bool VerifyPassword(LoginUserDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
                return false;
            

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return false;

            return Login(user.Id);
            

        }

        private bool Login(int userId)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == userId);

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
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(15),
                IsPersistent = true,
            };

            LoginAsync(new ClaimsPrincipal(claimsIdentity), authProperties);

            return true;
        }

        private async void LoginAsync(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authentication)
        {
            await _contextAccessor.HttpContext.SignInAsync(claimsPrincipal, authentication);
        }

   

    }


}
