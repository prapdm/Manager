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
    public class ServiceController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly ISerService _serService;
        private readonly ICategoryService _categoryService;

        public ServiceController(IFlashMessage flashMessage, ISerService serService,  ICategoryService categoryService)
        {

            _flashMessage = flashMessage;
            _serService = serService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Query query)
        {
            var categoryDtos = await _serService.GetAll(query);
            return View("Services", categoryDtos);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var service = await _serService.Get((int)id);

            if (service.CategoryId is not null)
            {
                var category = await _categoryService.Get((int)service.CategoryId);
                ViewData["CategoryName"] = category.Name;
            }
            else
                ViewData["CategoryName"] = "Null";

            if (service.PriceId is not null)
            {
                var price = await _serService.GetPirce((int)service.PriceId);
                ViewData["PurchasePrice"] = price.PurchasePrice;
                ViewData["SellPrice"] = price.SellPrice;
                ViewData["SKU"] = price.SKU;
            }
            else
            {
                ViewData["PurchasePrice"] = "Null";
                ViewData["SellPrice"] = "Null";
                ViewData["SKU"] = "Null";
            }

            return View("Details", service);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var service = await _serService.Get((int)id);
            ViewData["categories"] = await _categoryService.GetAll();
            return View("Edit", service);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Update(ServiceDto dto)
        {
            ViewData["categories"] = await _categoryService.GetAll();

            if (!ModelState.IsValid)
                return View("Edit", dto);

            _flashMessage.Info("Data successfully updated");
            var user = await _serService.Update(dto);
            return RedirectToAction("Edit", new { id = dto.Id });
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serService.Delete(id);
            _flashMessage.Info("Data successfully updated");
            return RedirectToAction("Get");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = await _categoryService.GetAll();
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceDto dto)
        {
            ViewData["categories"] = await _categoryService.GetAll();

            if (!ModelState.IsValid)
                return View("Create");

            _flashMessage.Info("Data successfully added");
            var category = await _serService.Create(dto);

            return View("Create");
        }




    }
}
