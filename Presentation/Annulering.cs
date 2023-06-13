class Annulering : IPresentation
{
    private static AnnuleringLogic annuleringLogic = new AnnuleringLogic();

    /// <summary>
    /// Generieke start functie.
    /// </summary>
    public static void Start()
    {

        PresentationLogic.CurrentPresentation = "annulering";

        string input = PresentationLogic.GetUserInputFromMenu(true).ToLower();
        switch (input)
        {
            case "r":
                Console.WriteLine("Weet u zeker dat u een annulering aan wilt vragen? y/n");
                for (int i = 0; i < 3; i++)
                {
                    Console.Write("> ");
                    string inputUser = Console.ReadLine().ToLower();
                    if (inputUser == "y")
                    {
                        if (AccountsLogic.CurrentAccount != null)
                        {
                            AnnuleringCode(AccountsLogic.CurrentAccount.EmailAddress);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Email");
                            string email = "";
                            for (int j = 0; j < 3; j++)
                            {
                                Console.Write("> ");
                                email = Console.ReadLine().ToLower();
                                if (MailLogic.ValidateMailAddress(email))
                                {
                                    AnnuleringCode(email);
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Geen geldig email adres");
                                    PresentationLogic.CurrentMessage = "Geen geldig email adres";
                                }
                            }
                            Console.WriteLine("Geen geldig email adres");
                            PresentationLogic.CurrentMessage = "Geen geldig email adres";
                            Menu.Start();
                            return;
                        }
                    }
                    else if (inputUser == "n")
                    {
                        Console.WriteLine("Uw annulering wordt niet verwerkt.");
                        PresentationLogic.CurrentMessage = "Annulering niet verwerkt";
                        Menu.Start();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Verkeerde invoer");
                        PresentationLogic.CurrentMessage = "Verkeerde invoer";
                    }
                }
                Menu.Start();
                return;
                // Console.WriteLine("Email");
                // Console.Write("> ");
                // string email = Console.ReadLine().ToLower();
                // AnnuleringCode(email);
                break;
            case "b":
                Menu.Start();
                return;
            case "o":
                AnnuleringAccepted();
                break;
            default:
                Console.WriteLine("Verkeerde invoer");
                PresentationLogic.CurrentMessage = "Verkeerde invoer";
                Menu.Start();
                return;
        }
    }

    /// <summary>
    /// Gebruiker input gebruiken om een reserveringscode te kunnen verwerken.
    /// </summary>
    public static void AnnuleringCode(string email)
    {
        List<ChairReservationModel> annulering = annuleringLogic.Annuleringen(email);
        List<MovieModel> movies = annuleringLogic.Movie();
        Console.WriteLine("Geef de reserveringscode van de tickets die u wilt annuleren.");
        Console.Write("> ");

        bool isValid = false;
        do
        {
            int id = Int32.TryParse(Console.ReadLine(), out id) ? id : -1;
            if (id < 0)
            {
                Console.WriteLine("Verkeerde invoer");
                PresentationLogic.CurrentMessage = "Verkeerde invoer";
                Menu.Start();
                return;
            }
            else
            {
                if (annuleringLogic.AnnuleringID(id, email))
                {
                    Console.WriteLine("Uw annulering wordt verwerkt.");
                    PresentationLogic.CurrentMessage = "Uw annulering wordt verwerkt.";
                }
                isValid = true;
            }
        }
        while (!isValid);

        if (isValid)
        {
            Menu.Start();
            return;
        }
    }

    /// <summary>
    /// Een lijst van annuleringen gebruiken om te bepalen welke annulering geaccepteerd/geweigerd kan worden.
    /// </summary>
    public static void AnnuleringAccepted()
    {
        annuleringLogic.ShowAnnulering();
        Console.WriteLine("Kies de ID die je wilt Gebruiken.");
        Console.Write(">");
        int idUser = -1;
        try
        {
            idUser = Convert.ToInt32(Console.ReadLine());
            AnnuleringModel annuleringsModel = annuleringLogic.GetAnnulering(idUser);
            if (annuleringsModel == null)
            {
                Console.WriteLine("Verkeerde invoer");
                PresentationLogic.CurrentMessage = "Verkeerde invoer";
                Annulering.Start();
                return;
            }
            bool acceptedAnnulering = annuleringLogic.AnnuleringAccept(idUser);
            Menu.Start();
            return;
        }
        catch (Exception e)
        {
            Menu.Start();
            return;
        }
    }
}