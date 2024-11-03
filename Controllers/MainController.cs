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

    [Route("NinjaList/CreateNinja")]
    public IActionResult CreateNinja()
    {
        return View("CreateOrEditNinja", new CreateOrEditNinjaViewModel(new Ninja()));
    }

    public IActionResult SaveNinja(CreateOrEditNinjaViewModel viewModel)
    {
        Ninja ninja = viewModel.Ninja;
        // Check for empty or invalid fields and add error messages to ModelState
        if (string.IsNullOrEmpty(ninja.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }
        else if (_context.Ninjas.Any(n => n.Name == ninja.Name))
        {
            ModelState.AddModelError("Name", "A ninja with this name already exists.");
        }

        if (ninja.Gold < 0)
        {
            ModelState.AddModelError("Gold", "Gold must be zero or a positive value.");
        }

        // If there are validation errors, return to the form view with validation messages
        if (!ModelState.IsValid)
        {
            return View("CreateOrEditNinja", new CreateOrEditNinjaViewModel(ninja));
        }

        // If all validation passes, save ninja and redirect to NinjaList
        _context.Ninjas.Add(ninja);
        _context.SaveChanges();
        return RedirectToAction("NinjaList");
    }

    [Route("NinjaList/{ninjaName}/Edit")]
    public IActionResult EditNinja(string ninjaName)
    {
        Ninja? ninja = _context.Ninjas.Include(n => n.NinjaInventories) // Load NinjaInventories collection
            .ThenInclude(ni => ni.Equipment).FirstOrDefault(n => n.Name == ninjaName);
        ViewData["IsEditMode"] = ninja != null;
        return View("CreateOrEditNinja", new CreateOrEditNinjaViewModel(ninja ?? new Ninja()));
    }

    public IActionResult UpdateNinja(Ninja ninja)
    {
        if (!ModelState.IsValid)
        {
            ViewData["IsEditMode"] = true;
            return View("CreateOrEditNinja", new CreateOrEditNinjaViewModel(ninja));
        }

        // Update logic here
        var existingNinja = _context.Ninjas.FirstOrDefault(n => n.Name == ninja.Name);
        if (existingNinja != null)
        {
            // Update properties
            existingNinja.Gold = ninja.Gold;

            _context.SaveChanges();
        }

        return RedirectToAction("NinjaList");
    }

    [Route("/NinjaList/Delete/{ninjaName}")]
    public IActionResult DeleteNinja(string ninjaName)
    {
        var ninja = _context.Ninjas.FirstOrDefault(n => n.Name == ninjaName);

        if (ninja != null)
        {
            var inventoriesToRemove = _context.NinjaInventories.Where(ni => ni.NinjaName == ninjaName);
            _context.NinjaInventories.RemoveRange(inventoriesToRemove);

            _context.Ninjas.Remove(ninja);
            _context.SaveChanges();
        }

        // Redirect to the "NinjaList" page
        return RedirectToAction("NinjaList");
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
        var ninja = _context.Ninjas.Include(n => n.NinjaInventories) // Load NinjaInventories collection
            .ThenInclude(ni => ni.Equipment).FirstOrDefault(n => n.Name == ninjaName);

        if (!string.IsNullOrEmpty(equipmentType))
        {
            // Perform filtering in memory
            var ninjaEquipmentNames = ninja.NinjaInventories.Select(ni => ni.Equipment.Name).ToList();

            // Filter the equipments based on the EquipmentType and exclude those already owned by the Ninja
            equipments = equipments
                .Where(e => e.EquipmentType.ToString() == equipmentType) // Filter by type
                .Where(e => !ninjaEquipmentNames.Contains(e.Name)) // Exclude already owned equipments
                .ToList();
            equipments = equipments.Where(e => e.EquipmentType.ToString() == equipmentType);
        }

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
        if (equipment.MonetaryValue <= 0)
        {
            ModelState.AddModelError("Equipment.MonetaryValue", "Monetary value must be greater than zero.");
        }

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
            ModelState.AddModelError("Equipment.Name", "Name is required.");
        }
        else if (_context.Equipments.Any(e => e.Name == equipment.Name))
        {
            ModelState.AddModelError("Equipment.Name", "An equipment with this name already exists.");
        }

        if (equipment.MonetaryValue <= 0)
        {
            ModelState.AddModelError("Equipment.MonetaryValue", "Monetary value must be greater than zero.");
        }

        // If there are validation errors, return to the form view with validation messages
        if (!ModelState.IsValid)
        {
            return View("CreateOrEditEquipment",
                new CreateOrEditEquipmentViewModel(viewModel.Ninja, equipment, viewModel.EquipmentType));
        }

        // If all validation passes, save equipment and redirect to Shop
        _context.Equipments.Add(equipment);
        _context.SaveChanges();
        if (viewModel.Ninja == null)
        {
            return RedirectToAction("Shop");
        }

        return RedirectToAction("Shop",
            new { ninjaName = viewModel.Ninja?.Name, equipmentType = viewModel.EquipmentType });
    }

    [Route("/NinjaList/NinjaView/{ninjaName}/Shop/Type/{equipmentType}/Sell/{equipmentName}")]
    public IActionResult RemoveEquipment(string ninjaName, string equipmentType, string equipmentName)
    {
        Ninja? ninja = _context.Ninjas.Include(n => n.NinjaInventories) // Load NinjaInventories collection
            .ThenInclude(ni => ni.Equipment).FirstOrDefault(n => n.Name == ninjaName);
        if (ninja != null)
        {
            var inventoriesToRemove = ninja.NinjaInventories
                .Where(ni =>
                    ni.Equipment.Name == equipmentName && ni.Equipment.EquipmentType.ToString() == equipmentType)
                .ToList();

            _context.NinjaInventories.RemoveRange(inventoriesToRemove);
            _context.SaveChanges();
        }

        return RedirectToAction("Shop", new { ninjaName, equipmentType });
    }

    [Route("NinjaList/NinjaView/{ninjaName}/Shop/Buy/{equipmentName}")]
    public IActionResult BuyEquipment(string ninjaName, string equipmentName, string equipmentType)
    {
        var ninjaEntity = _context.Ninjas
            .Include(n => n.NinjaInventories)
            .ThenInclude(ni => ni.Equipment)
            .FirstOrDefault(n => n.Name == ninjaName);
                           
        var equipment = _context.Equipments.AsNoTracking()
            .FirstOrDefault(e => e.Name == equipmentName);

        // Check if the ninja or equipment does not exist
        if (ninjaEntity == null || equipment == null)
        {
            TempData["ErrorMessage"] = "Invalid purchase attempt. Please try again.";
            return RedirectToAction("Shop", new { ninjaName, equipmentType });
        }

        // Check if ninja has enough gold to purchase
        if (ninjaEntity.Gold < equipment.MonetaryValue)
        {
            TempData["ErrorMessage"] = "Not enough gold to buy this equipment.";
            return RedirectToAction("Shop", new { ninjaName, equipmentType });
        }

        // Check if ninja already owns this equipment
        var ownsEquipment = ninjaEntity.NinjaInventories.Any(ni => ni.Equipment.Name == equipmentName);
        if (ownsEquipment)
        {
            TempData["ErrorMessage"] = "This equipment is already in your inventory.";
            return RedirectToAction("Shop", new { ninjaName, equipmentType });
        }
        
        // check how many equipment the ninja can have
        var maxEquipmentOfType = ninjaEntity.GetMaxEquipmentOfType(equipment.EquipmentType);
        var amountOfEquipmentOfType = ninjaEntity.NinjaInventories.Count(ni => ni.Equipment.EquipmentType == equipment.EquipmentType);
        if (amountOfEquipmentOfType >= maxEquipmentOfType)
        {
            TempData["ErrorMessage"] = "You can only have "+maxEquipmentOfType+" equipment at a time.";
            return RedirectToAction("Shop", new { ninjaName, equipmentType });
        }

        // Deduct gold and add equipment to ninja's inventory
        ninjaEntity.Gold -= equipment.MonetaryValue;
        ninjaEntity.NinjaInventories.Add(new NinjaInventory 
        { 
            Ninja = ninjaEntity, 
            EquipmentName = equipment.Name
        });

        _context.SaveChanges();

        // Redirect to NinjaView after successful purchase
        return RedirectToAction("NinjaView", new { ninjaName });
    }


    
}