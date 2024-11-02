using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            .Include(n => n.NinjaInventories)               // Load NinjaInventories collection
                .ThenInclude(ni => ni.Equipment)
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

    public IActionResult CreateEquipment()
    {
        return RedirectToAction("EditEquipment");
    }
    public IActionResult EditEquipment(string equipmentName)
    {
        var equipment = _context.Equipments.FirstOrDefault(e => e.Name == equipmentName);
        ViewData["IsEditMode"] = equipment != null;
        return View("CreateOrEditEquipment", equipment ?? new Equipment());
    }

    public IActionResult UpdateEquipment(Equipment equipment)
    {
        if (!ModelState.IsValid)
        {
            ViewData["IsEditMode"] = true;
            return View("CreateOrEditEquipment", equipment);
        }
    
        // Update logic here
        var existingEquipment = _context.Equipments.FirstOrDefault(e => e.Name == equipment.Name);
        if (existingEquipment != null)
        {
            // Update properties
            existingEquipment.EquipmentType = equipment.EquipmentType;
            existingEquipment.MonetaryValue = equipment.MonetaryValue;
            existingEquipment.Strength = equipment.Strength;
            existingEquipment.Intelligence = equipment.Intelligence;
            existingEquipment.Agility = equipment.Agility;

            _context.SaveChanges();
        }
    
        return RedirectToAction("Shop");
    }

    public IActionResult SaveEquipment(Equipment equipment)
    {
        // Check for empty or invalid fields and add error messages to ModelState
        if (string.IsNullOrEmpty(equipment.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }
        else if (_context.Equipments.Any(e => e.Name == equipment.Name))
        {
            ModelState.AddModelError("Name", "An equipment with this name already exists.");
        }

        if (equipment.MonetaryValue <= 0)
        {
            ModelState.AddModelError("MonetaryValue", "Monetary value must be greater than zero.");
        }

        if (equipment.Strength < 0)
        {
            ModelState.AddModelError("Strength", "Strength must be zero or a positive value.");
        }

        if (equipment.Intelligence < 0)
        {
            ModelState.AddModelError("Intelligence", "Intelligence must be zero or a positive value.");
        }

        if (equipment.Agility < 0)
        {
            ModelState.AddModelError("Agility", "Agility must be zero or a positive value.");
        }

        // If there are validation errors, return to the form view with validation messages
        if (!ModelState.IsValid)
        {
            return View("CreateOrEditEquipment", equipment);
        }

        // If all validation passes, save equipment and redirect to Shop
        _context.Equipments.Add(equipment);
        _context.SaveChanges();
        return RedirectToAction("Shop");
    }
}