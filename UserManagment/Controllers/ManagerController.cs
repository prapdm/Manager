using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Manager.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
 
            return View("Manager");

        }
    }
}
