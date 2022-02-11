using Manager.Models;
using Manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Manager.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    [AutoValidateAntiforgeryToken]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFlashMessage _flashMessage;

        public UsersController(IUserService userService, IFlashMessage flashMessage)
        {
            _userService = userService;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Query query)
        {
            var usersDtos = await _userService.GetAll(query);
            return View("Users", usersDtos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userService.GetUser(id);
            return View("Details", user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userService.EditUser(id);
            ViewData["roles"] = await _userService.GetRoles();
            return View("Edit", user);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["roles"] = await _userService.GetRoles();
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, Surname, Password, ConfirmPassword, Email, Phone, IsActive, RoleId")] RegisterUserDto dto)
        {
            ViewData["roles"] = await _userService.GetRoles();
            if (!ModelState.IsValid)
                return View("Create");

            _flashMessage.Info("Data successfully added");
            var user = await _userService.Create(dto);
            return View("Create");

        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Update(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["roles"] = await _userService.GetRoles();
                return View("Edit", dto);
            }

            _flashMessage.Info("Data successfully updated");
            var user = await _userService.Update(dto);
            return RedirectToAction("Edit", new { id = dto.Id });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            _flashMessage.Info("Data successfully updated");
            return RedirectToAction("Get");
        }
        

    }
}
