namespace PROG5_NinjaManager.Models;

public class EquipmentViewModel
{
    public EquipmentViewModel(Ninja ninja, Equipment? equipment, EquipmentType equipmentType)
    {
        Ninja = ninja;
        Equipment = equipment;
        EquipmentType = equipmentType;
    }
    public EquipmentViewModel(Ninja ninja, Equipment? equipment, EquipmentType equipmentType, bool clickable)
    {
        Ninja = ninja;
        Equipment = equipment;
        EquipmentType = equipmentType;
        Clickable = clickable;
    }
    public Equipment? Equipment { get; init; }
    public Ninja Ninja { get; init; }
    public EquipmentType EquipmentType { get; init; }
    public bool Clickable { get; init; } = true;
}