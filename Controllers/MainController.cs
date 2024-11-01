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

    [Route("NinjaList")]
    public IActionResult NinjaList()
    {
        return View(_context.Ninjas.ToList());
    }

    [Route("NinjaList/NinjaView/{ninjaName}")]
    public IActionResult NinjaView(string ninjaName)
    {
        var ninja = _context.Ninjas
            .FirstOrDefault(n => n.Name == ninjaName);

        if (ninja == null)
        {
            return RedirectToAction("NinjaList");
        }

        return View(ninja);
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