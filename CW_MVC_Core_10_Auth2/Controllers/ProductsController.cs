using CW_MVC_Core_10_Auth2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CW_MVC_Core_10_Auth2.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        //[Authorize(Policy = "DynamicRole")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create()
        {
            if (!RolesModerator.Roles.Any(role => User.IsInRole(role)))
            {
                return Forbid(); // Alternatively, you can redirect to an "Access Denied" page
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([Bind("Id,Name,Price,Description")] int id )
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }
    }
}
