using Microsoft.AspNetCore.Mvc;
using PROG5_NinjaManager.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PROG5_NinjaManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly MainContext _context;

        public HomeController(MainContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ninjas = _context.Ninjas.Include(n => n.Weapons).ToList();
            return View(ninjas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
