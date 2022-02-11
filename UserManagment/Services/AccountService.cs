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
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Services
{
    public interface IAccountService
    {
        Task<bool> VerifyPassword(LoginUserDto dto);
        Task RegisterUser(RegisterUserDto dto);
        Task<bool> VeryfiyEmail(VeryfiyEmailDto dto);
        Task Logout();
        Task<bool> ForgotPassword(VeryfiyEmailDto dto);
        Task<bool> VeryfiyToken(string token);
        Task<bool> ChangePassword(string token, ChangePasswordDto dto);


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
        public async Task<bool> VeryfiyEmail(VeryfiyEmailDto dto)
        {
            var user = await _context.Users
                .Where(u => u.Email == dto.Email)
                .Where(c => c.VerifcationCode == dto.VerifcationCode)
                .Where(c => c.IsActive == false)
                .FirstOrDefaultAsync();

            if (user is null)
                 return false;


            user.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RegisterUser([Bind("Email, Name, Surname, ConfirmPassword")] RegisterUserDto dto)
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

            var hashedPassword =  _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var email = new MailSenderDto()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                Subject = "Confirm your addres email",
                Template = "Welcome.cshtml",
                VerifcationCode = dto.VerifcationCode
            };

            await _mailSenderService.SendHtml(email);
 
        }

        public async Task<bool> VerifyPassword(LoginUserDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return false;

            return await Login(user.Id); 
        }


        public async Task<bool> VeryfiyToken(string token)
        {
            var user = await _context.Users
                .Where(d => d.UpdatedAt >= DateTime.Now.AddHours(-24) && d.UpdatedAt < DateTime.Now)
                .Where(u => u.VerifcationToken == token && u.VerifcationToken != null)
                .FirstOrDefaultAsync();

            if (user is null)
                return false;

            return true;
        }

        public async Task<bool> ChangePassword(string token, ChangePasswordDto dto)
        {
            var user = await _context.Users
                .Where(d => d.UpdatedAt >= DateTime.Now.AddHours(-24) && d.UpdatedAt < DateTime.Now)
                .Where(u => u.VerifcationToken == token && u.VerifcationToken != null)
                .FirstOrDefaultAsync();

            if (user is null)
                return false;

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            user.VerifcationToken = null;
            await _context.SaveChangesAsync();

            return true;
        }
             

        public async Task<bool> ForgotPassword(VeryfiyEmailDto dto)
        {
            var user = await _context.Users
                .Where(u => u.Email == dto.Email)
                .Where(c => c.IsActive == true)
                .FirstOrDefaultAsync();

            if (user is null)
                return false;

            var token = RandomString(35);
            user.VerifcationToken = token;
            await _context.SaveChangesAsync();

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

            await _mailSenderService.SendHtml(email);

            return true;
        }

 


        private async Task<bool> Login(int userId)
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

            await LoginAsync(new ClaimsPrincipal(claimsIdentity), authProperties);

            return true;
        }


        public async Task Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync();
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task LoginAsync(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authentication)
        {
            await _contextAccessor.HttpContext.SignInAsync(claimsPrincipal, authentication);
        }


    }


}
