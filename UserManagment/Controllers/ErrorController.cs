using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Manager.Exeptions
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Exeption()
        {
            return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Code(int id)
        {
            TempData["id"] = id;
            return View();
        }


    }
}
