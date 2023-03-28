static class Auditorium
{
    private static AuditoriumLogic _auditoriumLogic;

    public static void Start()
    {
        Console.WriteLine("Please enter the auditorium number");
        Console.Write(">");
        int auditoriumID = int.Parse(Console.ReadLine());
        _auditoriumLogic = new AuditoriumLogic(auditoriumID);
        PrintChairs();
    }

    public static void PrintChairs()
    {
        _auditoriumLogic.ChairPrint();
    }
}