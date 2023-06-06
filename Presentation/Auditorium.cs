public class Auditorium : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();
    private static ChairReservationLogic _chairReservationLogic = new ChairReservationLogic();
    private static ChairLogic _chairLogic = new ChairLogic();

    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "auditorium";

        _auditoriumLogic.ChairPrint(0, 0);
        //scherm weergeven
        AudistoriumScreen();
        //legenda weergeven
        PrintLegenda();
        string chosenOption = PresentationLogic.GetUserInputFromMenu(true);
        if (chosenOption == "b")
        {
            Movie.Start();
        }
        else if (chosenOption == "s")
        {
            ChooseChair();
        }
        else
        {
            Console.WriteLine("Incorrecte invoer.");
            Movie.Start();
        }
    }

    public static void ChooseChair()
    {
        // de beginende axises. 
        int indexX = 2;
        int indexY = 2;
        // de stoelen van dit auditorium om te kunnen gebruiken om te weten op welke je op
        // dit moment zit met je cursor.
        List<int> chairIDs = AuditoriumLogic.GetChairIDs(Movie.AuditoriumID);
        _auditoriumLogic.ChairPrint(indexY, indexX);
        AuditoriumModel auditoriumModel = _auditoriumLogic.GetAuditoriumModel(Movie.AuditoriumID);
        // lijst met de gekozen stoelen van de gebruiker om ze allemaal te kunnen reserveren
        List<ChairModel> chosenChairs = new List<ChairModel>();
        ConsoleKeyInfo keyinfo;
        bool isBackKey = false;
        do
        {
            // clear de console en print alle belangrijke dingen.
            Console.Clear();
            _auditoriumLogic.ChairPrint(indexX, indexY);
            AudistoriumScreen();
            PrintLegenda();
            // zodat de gebruiker kan weten wat hij/zij geselecteerd heeft
            PrintChosenChairs(chosenChairs);

            // lees de key om te bepalen wat de gebruiker doet.
            keyinfo = Console.ReadKey();

            switch (keyinfo.Key)
            {
                // alle up/down/left/right dingen checken of die kan bewegen
                // zo niet doe niks
                case ConsoleKey.DownArrow:
                    if (indexX + 1 < auditoriumModel.TotalRows + 1)
                        indexX++;
                    break;
                case ConsoleKey.UpArrow:
                    if (indexX - 1 >= 1)
                        indexX--;
                    break;
                case ConsoleKey.RightArrow:
                    if (indexY + 1 < auditoriumModel.TotalCols + 1)
                        indexY++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (indexY - 1 >= 1)
                        indexY--;
                    break;
                // enter om een keuze te kunnen maken
                case ConsoleKey.Enter:
                    // zoek de id van de stoel die gebruikt kan worden om de daadwerklijke stoel te vinden.
                    int chairID = ChairLogic.FindChairID(indexX - 1, indexY, Movie.AuditoriumID);
                    if (chairID == 0)
                    {
                        Console.WriteLine("Stoel met die coordinaten is niet gevonden. Of is niet te kiezen omdat het wit is.");
                        break;
                    }
                    // stoel model om te gebruiken in de lijst die uiteindelijk gereserveerd wordt.
                    ChairModel chair = _chairLogic.GetChairModel(chairID);
                    // als een stoel weer word gekozen kan die uit de lijst gehaald worden.
                    if (chosenChairs.Contains(chair))
                    {
                        chosenChairs.Remove(chair);
                        break;
                    }
                    // als de kleur wit is mag die niet gekozen worden door de gebruiker
                    if (chair.Color == "White")
                        break;
                    chosenChairs.Add(chair);
                    break;
                // s is om op te slaan
                case ConsoleKey.S:
                    break;
                // b is om terug te gaan.
                case ConsoleKey.B:
                    break;
            }
        }
        while (keyinfo.Key != ConsoleKey.S && keyinfo.Key != ConsoleKey.B);
        // als je b hebt gedrukt moet je terug naar het film overzicht.
        bool areGoodToReserve = true;
        if (keyinfo.Key == ConsoleKey.B)
        {
            areGoodToReserve = false;
            Movie.Start();
        }

        Console.Clear();
        _auditoriumLogic.ChairPrint(0, 0);
        // de gebruiker laten weten dat hij/zij wel een stoel moet kiezen en dus niet verder kan.
        if (chosenChairs.Count() <= 0)
        {
            Console.WriteLine("U heeft geen stoelen geselecteerd, u moet of minimaal een selecteren.");
            Console.WriteLine();
            Movie.Start();
            return;
        }

        // stop de chosenChairs in een public static variable om te gebruiken in andere classes
        AccountsLogic.ChosenChairs = chosenChairs;
        _chairReservationLogic = new ChairReservationLogic();

        // loop door alle stoelen heen om die te reserveren.
        foreach (ChairModel _chair in chosenChairs)
        {
            //Maakt een reservering terwel die de gegevens controleert 
            bool ret = _chairReservationLogic.ReserveChair(Movie.AuditoriumID, Movie.MovieID, _chair);
            // als een van de stoelen al gereserveerd is oid, ga je terug naar het menu want de stoel is niet meer
            // beschikbaar.
            if (ret == false)
            {
                areGoodToReserve = false;
                Console.WriteLine("Stoel is niet beschikbaar");
                Auditorium.Start();
                return;
            }
        }

        if (areGoodToReserve == true)
        {
            Console.WriteLine("Stoel of stoelen gereserveerd!");
            Auditorium.ChooseCombi();
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
        AuditoriumModel auditorium = _auditoriumLogic.GetAuditoriumModel(Movie.AuditoriumID);
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
        Console.WriteLine("De stoelen waar X op staat zijn bezet.");
        Console.WriteLine("De stoelen waar ? op staat zijn tijdelijk bezet.");
        Console.WriteLine("+------------------------------------------------------------------------+");
        Console.WriteLine("Info over het selecteren: ");
        Console.WriteLine("Met de pijltjes kunnen er stoelen gekozen worden door op enter te drukken");
        Console.WriteLine("als u tevreden bent met de selectie kan er op 'S' gedrukt worden\nom uw keuze op te slaan.");
    }

    /// <summary>
    /// Method om de geselecteerde stoelen te printen, als de lijst met chairs vol is.
    /// </summary>
    public static void PrintChosenChairs(List<ChairModel> chairs)
    {
        if (chairs.Count() <= 0)
            return;
        Console.Write("Geselecteerde stoelen: ");
        foreach (ChairModel _chair in chairs)
        {
            char col;
            if (_chair.Column > 26)
                col = Convert.ToChar(_chair.Column + 71);
            else
                col = Convert.ToChar(_chair.Column + 65);
            Console.Write($"{col}, {_chair.Row + 1}; ");
        }
    }
}