using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG5_NinjaManager.Models;

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
            .Include(n => n.NinjaInventories) // Load NinjaInventories collection
            .ThenInclude(ni => ni.Equipment)
            .FirstOrDefault(n => n.Name == ninjaName);

        if (ninja == null)
        {
            return RedirectToAction("NinjaList");
        }

        return View(ninja);
    }

    [Route("Shop")]
    public IActionResult Shop()
    {
        IEnumerable<Equipment> equipments = _context.Equipments.AsEnumerable();
        return View(new ShopViewModel(equipments));
    }

    [Route("NinjaList/NinjaView/{ninjaName}/Shop/Type/{equipmentType}")]
    public IActionResult Shop(string ninjaName, string equipmentType)
    {
        var equipments = _context.Equipments.AsEnumerable(); // Retrieve data from database first
        var ninja = _context.Ninjas.FirstOrDefault(n => n.Name == ninjaName);

        if (!string.IsNullOrEmpty(equipmentType))
        {
            // Perform filtering in memory
            equipments = equipments.Where(e => e.EquipmentType.ToString() == equipmentType);
        }

        ViewBag.FilterType = equipmentType; // Pass selected filter to the view
        
        return View(new ShopViewModel(equipments, ninja, equipmentType));
    }

    [Route("Shop/Equipment/{equipmentName?}")]
    public IActionResult EditEquipment(string equipmentName)
    {
        Equipment? equipment = _context.Equipments.Include(n => n.NinjaInventories) // Load NinjaInventories collection
            .ThenInclude(ni => ni.Ninja).FirstOrDefault(e => e.Name == equipmentName);
        ViewData["IsEditMode"] = equipment != null;
        return View("CreateOrEditEquipment", new CreateOrEditEquipmentViewModel(equipment ?? new Equipment()));
    }
    
    [Route("NinjaList/NinjaView/{ninjaName}/Shop/Type/{equipmentTypeString}/Equipment/")]
    public IActionResult EditEquipment(string ninjaName, string equipmentTypeString)
    {
        ViewData["IsEditMode"] = false;
        Ninja? ninja = _context.Ninjas.FirstOrDefault(n => n.Name == ninjaName);
        EquipmentType equipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType), equipmentTypeString);
        return View("CreateOrEditEquipment", new CreateOrEditEquipmentViewModel(ninja, new Equipment(), equipmentType));
    }

    public IActionResult UpdateEquipment(Equipment equipment)
    {
        if (!ModelState.IsValid)
        {
            ViewData["IsEditMode"] = true;
            return View("CreateOrEditEquipment", new CreateOrEditEquipmentViewModel(equipment));
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

    [Route("/Shop/Delete/{equipmentName}")]
    public IActionResult DeleteEquipment(string equipmentName)
    {
        var equipment = _context.Equipments.FirstOrDefault(e => e.Name == equipmentName);

        if (equipment != null)
        {
            var inventoriesToRemove = _context.NinjaInventories.Where(ni => ni.EquipmentName == equipmentName);
            _context.NinjaInventories.RemoveRange(inventoriesToRemove);
            
            _context.Equipments.Remove(equipment);
            _context.SaveChanges();
        }

        // Redirect to the "Shop" page
        return RedirectToAction("Shop");
    }

    public IActionResult SaveEquipment(CreateOrEditEquipmentViewModel viewModel)
    {
        Equipment equipment = viewModel.Equipment;
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
            return View("CreateOrEditEquipment", new CreateOrEditEquipmentViewModel(viewModel.Ninja, equipment, viewModel.EquipmentType));
        }

        // If all validation passes, save equipment and redirect to Shop
        _context.Equipments.Add(equipment);
        _context.SaveChanges();
        return RedirectToAction("Shop", new { ninjaName = viewModel.Ninja?.Name, equipmentType = viewModel.EquipmentType});
    }
}