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

    public IActionResult Shop(string? filterType = null)
    {
        var equipment = _context.Equipments.AsEnumerable(); // Retrieve data from database first

        if (!string.IsNullOrEmpty(filterType))
        {
            // Perform filtering in memory
            equipment = equipment.Where(e => e.EquipmentType.ToString() == filterType);
        }

        ViewBag.FilterType = filterType; // Pass selected filter to the view
        return View(equipment.ToList());
    }

}