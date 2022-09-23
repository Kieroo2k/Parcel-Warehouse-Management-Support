using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParcelWarehouseManagementSupport.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelWarehouseManagementSupport.Controllers
{
    [RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Couriers");
            }
            return RedirectToAction("Index", "Deliveries");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
