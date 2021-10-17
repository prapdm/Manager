using Microsoft.AspNetCore.Mvc;
using Manager.Models;
using Manager.Services;
using Vereyon.Web;
using Microsoft.Extensions.Logging;
using System;

namespace Manager.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFlashMessage _flashMessage;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IFlashMessage flashMessage, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _flashMessage = flashMessage;
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Login()
        {

            if(!User.Identity.IsAuthenticated)
            {
                return View("Login");
            }
            return RedirectToAction("index","manager");

        }

        [HttpPost]
        public IActionResult LoginUser([FromForm] LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return View("Login");

            var result = _accountService.LoginUser(dto);
            if(result)
                return RedirectToAction("index", "manager");
            else
            {
                _flashMessage.Danger("Invalid username or password");
                return RedirectToAction("Login");
            }
                
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public IActionResult RegisterUser([FromForm] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            return View("Register");

            _accountService.RegisterUser(dto);
            return RedirectToAction("Login");
        }

        

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }


    }
}
