class Auditorium : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();
    private static ChairReservationLogic _chairReservationLogic = new ChairReservationLogic();

    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "auditorium";

        PrintChairs();
        //scherm weergeven
        AudistoriumScreen();
        //legenda weergeven
        PrintLegenda();
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
            CombiDeal.Start();
        }
        else if (choisCombiDeals == "n")
        {
            Auditorium.ChooseParkingTicket();
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
                ParkingTicketLogic.choiseParkingTicket = true;
                ParkingTicketLogic.GenerateParkingTicket();
                choice = true;
                AccountsLogic.TotaalPrijs += 2;
                Console.WriteLine("U heeft een parkeer ticket gekocht.");
                Payment.Start();
            }
            else if (choiseParkingTicket == "n")
            {
                choice = true;
                Payment.Start();
            }
            else
            {
                Console.WriteLine("Geen correcte invoer.");
                continue;
            }
        }
    }

    public static void AudistoriumScreen()
    {
        AuditoriumModel auditorium = _auditoriumLogic.GetAuditoriumModel(Movie.AuditoriumID + 1);
        if (auditorium != null)
        {
            Console.Write("\\");
            for (int i = 0; i < auditorium.TotalCols; i++)
            {
                Console.Write("__");
            }
            Console.Write("/\n\n");
        }
    }

    public static void PrintLegenda()
    {
        Console.WriteLine("Stoelkosten:");
        Console.WriteLine("Blauw: 5,-  | Oranje: 10,-");
        Console.WriteLine("Rood : 15,- | Grijs : Niet beschikbaar.");
        Console.WriteLine("De stoelen waar X op staat is bezet.");
    }
}