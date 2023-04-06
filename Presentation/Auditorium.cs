class Auditorium : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();

    public static void Start()
    {
        PrintChairs();
    }

    public static void PrintChairs()
    {
        _auditoriumLogic.ChairPrint();
    }
}