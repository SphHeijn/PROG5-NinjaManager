namespace PROG5_NinjaManager.Models;

public class EquipmentViewModel(Ninja ninja, Equipment? equipment, EquipmentType equipmentType)
{
    public Equipment? Equipment { get; init; } = equipment;
    public Ninja Ninja { get; init; } = ninja;
    public EquipmentType EquipmentType { get; init; } = equipmentType;
}