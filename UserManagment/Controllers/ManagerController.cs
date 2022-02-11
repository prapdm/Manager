using Manager.Models;
using Manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Manager.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ManagerController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFlashMessage _flashMessage;

        public ManagerController(IUserService userService, IFlashMessage flashMessage)
        {
            _userService = userService;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Manager");
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var usersDto = await _userService.Get();
            return View("Profile", usersDto);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromForm] UserDto dto)
        {
            await _userService.SaveProfile(dto);
            _flashMessage.Info("Data successfully updated");
            return RedirectToAction("Profile");
        }
    }
}
