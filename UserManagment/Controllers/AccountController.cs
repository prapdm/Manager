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
   

        public AccountController(IAccountService accountService, IFlashMessage flashMessage)
        {
            _accountService = accountService;
            _flashMessage = flashMessage;
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

            var result = _accountService.VerifyPassword(dto);
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
            _flashMessage.Info("Check your email account to confirm your email address.");
            return RedirectToAction("VeryfiyEmail");
        }

        [HttpGet]
        public IActionResult VeryfiyEmail()
        {
            return View("VeryfiyEmail");
        }

        [HttpPost]
        public IActionResult VeryfiyEmail([FromForm] VeryfiyEmailDto dto)
        {
            if (!ModelState.IsValid)
                return View("VeryfiyEmail");

            var result = _accountService.VeryfiyEmail(dto);
            if (!result)
            {
                _flashMessage.Info("Verification code or email is incorect.");
                return RedirectToAction("VeryfiyEmail");
            }
            else
            {
                _flashMessage.Info("Your account is active now. You can log in.");
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }


    }
}
