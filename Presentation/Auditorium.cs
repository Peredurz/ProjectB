public class Auditorium
{
    AuditoriumLogic _auditoriumLogic;

    public Auditorium(int auditoriumID)
    {
        _auditoriumLogic = new AuditoriumLogic(auditoriumID);
    }

    public void PrintChairs()
    {
        Console.WriteLine(_auditoriumLogic.ChairPrint());
    }
}