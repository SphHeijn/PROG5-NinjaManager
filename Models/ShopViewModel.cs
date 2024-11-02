namespace PROG5_NinjaManager.Models;

public class ShopViewModel(IEnumerable<Equipment> equipments, string? filterType)
{
    public IEnumerable<Equipment> Equipments { get; set; } = equipments;
    public string? FilterType { get; set; } = filterType;
}