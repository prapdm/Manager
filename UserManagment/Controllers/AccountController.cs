using Microsoft.AspNetCore.Mvc;
using Manager.Models;
using Manager.Services;
using Vereyon.Web;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

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
        public IActionResult ChangePassword([FromForm] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                _flashMessage.Warning("Check if passwords are the same and email address is valid");
                return RedirectToAction("ForgotPassword", new { token = dto.VerifyToken });
            }
               

            var result = _accountService.ChangePassword(dto.VerifyToken, dto);
            if (!result)
            {
                _flashMessage.Info("The token is invalid or has expired");
                return RedirectToAction("Login");
            }
            _flashMessage.Info("Password successfully updated");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult VeryfiyEmail([FromForm] VeryfiyEmailDto dto)
        {
 
            var result = _accountService.VeryfiyEmail(dto);
            if (!result)
            {
                _flashMessage.Warning("Verification code or email is incorect.");
                return RedirectToAction("VeryfiyEmail");
            }
            else
            {
                _flashMessage.Info("Your account is active now. You can log in.");
                return RedirectToAction("Login");
            }

        }

 


        [HttpGet]
        public IActionResult ForgotPassword([FromQuery] string token)
        {
            if(token is null)
                return View("ForgotPassword");

            var result = _accountService.VeryfiyToken(token);
            if (result)
                return View("ChangePassword");

            
            _flashMessage.Warning("The token is invalid or has expired");
            return RedirectToAction("Login");
            

           
        }

        [HttpPost]
        public IActionResult ForgotPassword([FromForm] VeryfiyEmailDto dto)
        {
            var result = _accountService.ForgotPassword(dto);
            if (result)
            {
                _flashMessage.Info("Check your email account to reset password.");
                return RedirectToAction("Login");
            }

            _flashMessage.Info("If account exist, you should recive email with reset link");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _accountService.Logout();
            return RedirectToAction("Login");
        }


    }
}
