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
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace Manager.Services
{
    public interface IAccountService
    {
        bool VerifyPassword(LoginUserDto dto);
        void RegisterUser(RegisterUserDto dto);
        bool VeryfiyEmail(VeryfiyEmailDto dto);
        void Logout();
        bool ForgotPassword(VeryfiyEmailDto dto);
        bool VeryfiyToken(string token);
        bool ChangePassword(string token, ChangePasswordDto dto);


    }
    public class AccountService : IAccountService
    {
        private readonly ManagerDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailSenderService _mailSenderService;
        private readonly static Random random = new Random();
        public AccountService(ManagerDbContext context, IPasswordHasher<User> passwordHasher, 
            IHttpContextAccessor contextAccessor, IMailSenderService mailSenderService   )
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
            _mailSenderService = mailSenderService;
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
            var newUser = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                RoleId = dto.RoleId,
                VerifcationCode = dto.VerifcationCode,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var email = new MailSenderDto()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                Subject = "Confirm your addres email",
                Template = "Welcome.cshtml",
                VerifcationCode = dto.VerifcationCode
            };

            _mailSenderService.SendHtml(email);
 
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


        public bool VeryfiyToken(string token)
        {
            var user = _context.Users
                .Where(d => d.UpdatedAt >= DateTime.Now.AddHours(-24) && d.UpdatedAt < DateTime.Now)
                .Where(u => u.VerifcationToken == token && u.VerifcationToken != null)
                .FirstOrDefault();

            if (user is null)
                return false;

            return true;
        }

        public bool ChangePassword(string token, ChangePasswordDto dto)
        {
            var user = _context.Users
                .Where(d => d.UpdatedAt >= DateTime.Now.AddHours(-24) && d.UpdatedAt < DateTime.Now)
                .Where(u => u.VerifcationToken == token && u.VerifcationToken != null)
                .FirstOrDefault();

            if (user is null)
                return false;

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            _context.SaveChanges();

            return true;
        }
             

        public bool ForgotPassword(VeryfiyEmailDto dto)
        {
            var user = _context.Users
                .Where(u => u.Email == dto.Email)
                .Where(c => c.IsActive == true)
                .FirstOrDefault();

            if (user is null)
                return false;

            var token = RandomString(35);
            user.VerifcationToken = token;
            _context.SaveChanges();

            var url = _contextAccessor.HttpContext?.Request?.GetDisplayUrl();

            var email = new MailSenderDto()
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Subject = "Reset password",
                Template = "ResetPassword.cshtml",
                Url = $"{url}?token={token}"
            };

            _mailSenderService.SendHtml(email);

            return true;
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


        public async void Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync();
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void LoginAsync(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authentication)
        {
            await _contextAccessor.HttpContext.SignInAsync(claimsPrincipal, authentication);
        }


    }


}
