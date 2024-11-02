namespace PROG5_NinjaManager.Models;

public class EquipmentViewModel(Ninja ninja, Equipment? equipment, EquipmentType equipmentType)
{
    public Equipment? Equipment { get; set; } = equipment;
    public Ninja Ninja { get; set; } = ninja;
    public EquipmentType EquipmentType { get; set; } = equipmentType;
}