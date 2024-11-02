namespace PROG5_NinjaManager.Models;

public class CreateOrEditNinjaViewModel
{
    public CreateOrEditNinjaViewModel(Ninja ninja)
    {
        Ninja = ninja;
    }

    public CreateOrEditNinjaViewModel()
    {
        Ninja = new Ninja();
    }
    public Ninja Ninja { get; init; }
}