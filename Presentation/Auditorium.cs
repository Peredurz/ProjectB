class Auditorium : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();
    private static ChairReservationLogic _chairReservationLogic = new ChairReservationLogic();

    public static void Start()
    {
        PrintChairs();
        Console.WriteLine("Wil je een stoel kiezen? Of terug?");
        Console.WriteLine("B: Terug\nS: Stoel kiezen");
        Console.Write("> ");
        string chosenOption = Console.ReadLine().ToLower();
        if (chosenOption == "b")
            Menu.Start();
        else if (chosenOption == "s")
            ChooseChair();
        else
            Console.WriteLine("Incorrecte invoer.");
    }

    public static void PrintChairs()
    {
        _auditoriumLogic.ChairPrint();
    }

    public static void ChooseChair()
    {
        try
        {
            Console.WriteLine("Kies een rij (1, 2, 3, ...):");
            Console.Write("> ");
            int chosenRow = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Kies een stoel (A, B, C, ...):");
            Console.Write("> ");
            char chosenChair = Convert.ToChar(Console.ReadLine());
            Console.WriteLine($"Je hebt gekozen voor stoel: {chosenChair}-{chosenRow}");
            // Convert de ascii karakter naar een getal.
            int chosenChairNumber = (int)chosenChair - 64;
            // Als het kleine letters is moet het een ander getal zijn om er af te halen
            if (chosenChairNumber > 26)
                chosenChairNumber = (int)chosenChair - 70;
            //Maakt een reservering terwel die de gegevens controleert 
            bool ret = _chairReservationLogic.ReserveChair(Movie.AuditoriumID, Movie.MovieID, chosenRow - 1, chosenChairNumber);
            if (ret == false)
            {
                Console.WriteLine("Stoel is niet beschikbaar");
                return;
            }
            else
            {
                Console.WriteLine("Stoel gereserveerd!");
                Auditorium.ChooseCombi();
            }
        } catch (FormatException ex)
        {
            Console.WriteLine("Je moet een nummer invoeren");
        }
    }
    public static void ChooseCombi()
    {
        Console.WriteLine($"Wilt u de Combi deals bekijken. Y/N");
        Console.Write(">");
        string choisCombiDeals = Console.ReadLine().ToLower();
        if (choisCombiDeals == "y")
        {
            Console.WriteLine("De Combi Deals zijn nog niet beschikbaar.");
        }
        else if (choisCombiDeals == "n")
        {
            return;
        }
    }
}