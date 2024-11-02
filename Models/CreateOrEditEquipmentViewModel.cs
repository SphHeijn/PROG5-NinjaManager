namespace PROG5_NinjaManager.Models;

public class CreateOrEditEquipmentViewModel
{
    public CreateOrEditEquipmentViewModel(Ninja? ninja, Equipment equipment, EquipmentType? equipmentType)
    {
        Ninja = ninja;
        Equipment = equipment;
        EquipmentType = equipmentType;
    }
    public CreateOrEditEquipmentViewModel(Equipment equipment)
    {
        Equipment = equipment;
    }
    

    public Equipment Equipment { get; init; }
    public Ninja? Ninja { get; init; }
    public EquipmentType? EquipmentType { get; init; }
}