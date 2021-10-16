using Microsoft.AspNetCore.Mvc;
using Manager.Models;
using Manager.Services;



namespace Manager.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login([FromForm] LoginUserDto dto)
        {
            _accountService.LoginUser(dto);
            return RedirectToAction("Login");
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
