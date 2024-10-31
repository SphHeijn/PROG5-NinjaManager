using Microsoft.AspNetCore.Mvc;

namespace PROG5_NinjaManager.Controllers;

public class MainController : Controller
{
    private readonly MainContext _context;

    public MainController(MainContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return RedirectToAction("NinjaList");
    }

    public IActionResult NinjaList()
    {
        return View(_context.Ninjas.ToList());
    }

    public IActionResult Shop()
    {
        return View(_context.Equipments.ToList());
    }
}