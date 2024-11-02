namespace PROG5_NinjaManager.ViewModels;

public class ShopViewModel
{
    // This class is used to pass data from the controller to the view
    public string? FilterType { get; set; }
    public IEnumerable<Equipment> Equipments { get; set; }
    
    //function to receive a filter and show the equipment with that filter
    public IEnumerable<Equipment> FilteredEquipments()
    {
        if (string.IsNullOrEmpty(FilterType))
        {
            return Equipments;
        }
        return Equipments.Where(e => e.EquipmentType.ToString() == FilterType);
    }
}