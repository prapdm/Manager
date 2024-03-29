﻿using Manager.Models;
using Manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Manager.Controllers
{
  
    [AutoValidateAntiforgeryToken]
    public class CategoryController : Controller
    {

        private readonly IFlashMessage _flashMessage;
        private readonly ICategoryService _categoryService;

        public CategoryController( IFlashMessage flashMessage, ICategoryService categoryService)
        {
         
            _flashMessage = flashMessage;
            _categoryService = categoryService;
         
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, User")]
        public async Task<IActionResult> Get([FromQuery] Query query)
        {
            var categoryDtos = await _categoryService.GetAll(query);
            return View("Categories", categoryDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, User")]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = await _categoryService.GetAll();
            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            ViewData["categories"] = await _categoryService.GetAll();

            if (!ModelState.IsValid)
                return View("Create");

            _flashMessage.Info("Data successfully added");
            var category = await _categoryService.Create(dto);
            
            return View("Create");
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            _flashMessage.Info("Data successfully updated");
            return RedirectToAction("Get");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, User")]
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryService.Get((int)id);
            ViewData["categories"] = await _categoryService.GetAll();
            return View("Edit", category);
        }

        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            ViewData["categories"] = await _categoryService.GetAll();

            if (!ModelState.IsValid)
                return View("Edit", dto);
             
            _flashMessage.Info("Data successfully updated");
            var user = await _categoryService.Update(dto);
            return RedirectToAction("Edit", new { id = dto.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, User")]
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoryService.Get((int)id);
            if (category.ParentId is not null)
            {
                var subcategory = await _categoryService.Get((int)category.ParentId);
                ViewData["SubcategoryName"] = subcategory.Name;
            }
            else
                ViewData["SubcategoryName"] = "Null";

            return View("Details", category);
        }

    }
}
