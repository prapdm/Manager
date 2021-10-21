using Manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Manager.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ManagerController : Controller
    {
        private readonly IUserService _userService;

        public ManagerController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Manager");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Users()
        {
           var usersDtos = _userService.GetAll();
           return View("Users", usersDtos);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View("Profile");
        }

    }
}
