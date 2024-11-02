namespace PROG5_NinjaManager.Models;

public class ShopViewModel
{
    public ShopViewModel (IEnumerable<Equipment>equipments, Ninja? ninja, string? filterType)
    {
        Equipments = equipments;
        Ninja = ninja;
        FilterType = filterType;
    }

    public ShopViewModel(IEnumerable<Equipment> equipments)
    {
        Equipments = equipments;
    }
    public IEnumerable<Equipment> Equipments { get; init; }
    public Ninja? Ninja { get; init; }
    public string? FilterType { get; init; }
}