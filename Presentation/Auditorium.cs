class Auditorium : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();
    private static ChairReservationLogic _chairReservationLogic = new ChairReservationLogic();

    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "auditorium";

        PrintChairs();
        PresentationLogic.WriteMenu(Menu.presentationModels, true);
        string chosenOption = Console.ReadLine().ToLower();
        if (chosenOption == "b")
        {
            Menu.Start();
        }
        else if (chosenOption == "s")
        {
            ChooseChair();
        }
        else
        {
            Console.WriteLine("Incorrecte invoer.");
            Menu.Start();
        }
    }

    public static void PrintChairs()
    {
        _auditoriumLogic.ChairPrint();
    }

    public static void ChooseChair()
    {
        try
        {
            bool finalDecision = false;
            int chosenRow;
            char chosenChair;
            do
            {
                Console.WriteLine("Kies een rij (1, 2, 3, ...):");
                Console.Write("> ");
                chosenRow = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Kies een stoel (A, B, C, ...):");
                Console.Write("> ");
                chosenChair = Convert.ToChar(Console.ReadLine());
                Console.WriteLine("Weet je zeker dat je deze stoelen wilt selecteren? Y/N");
                string userOption = Console.ReadLine().ToLower();
                if (userOption == "y")
                {
                    Console.WriteLine($"Je hebt gekozen voor stoel: {chosenChair}-{chosenRow}");
                    finalDecision = true;
                }
                else if (userOption == "n")
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Geen correcte invoer, vul uw rij en stoel opnieuw in.");
                    continue;
                }
            }
            while (finalDecision == false);
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
                Menu.Start();
            }
            else
            {
                Console.WriteLine("Stoel gereserveerd!");
                Auditorium.ChooseCombi();
            }
        }
        catch (FormatException ex)
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
            Menu.Start();
        }
    }

    public static void ChooseParkingTicket()
    {
        Console.WriteLine($"Wilt u een parkeer ticket kopen. Y/N");
        Console.Write(">");
        bool choice = false;
        string choiseParkingTicket = Console.ReadLine().ToLower();
        while (choice == false)
        {
            if (choiseParkingTicket == "y")
            {
                ParkingTicketLogic.GenerateParkingTicket();
            }
            else if (choiseParkingTicket == "n")
            {
                Menu.Start();
            }
            else
            {
                Console.WriteLine("Geen correcte invoer.");
                continue;
            }
        }
    }
}