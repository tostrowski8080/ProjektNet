using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkshopManager.Models;

namespace WorkshopManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Register");
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToPage("/Dashboard/Admin");
            }

            if (User.IsInRole("Mechanic"))
            {
                return RedirectToPage("/Dashboard/Mechanic");
            }

            if (User.IsInRole("Receptionist"))
            {
                return RedirectToPage("/Dashboard/Receptionist");
            }

            if (User.IsInRole("Client"))
            {
                return RedirectToPage("/Dashboard/Client");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
