class Annulering : IPresentation
{
    private static AnnuleringLogic annuleringLogic = new AnnuleringLogic();

    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "annulering";
        PresentationLogic.WriteMenu(Menu.presentationModels, true);

        string input = Console.ReadLine().ToLower();
        switch (input)
        {
            case "r":
                Console.WriteLine("Email");
                Console.Write("> ");
                string email = Console.ReadLine().ToLower();
                AnnuleringCode(email);
                break;
            case "b":
                Menu.Start();
                break;
            default:
                Console.WriteLine("Verkeerde invoer");
                Menu.Start();
                break;
        }
    }

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
            }
            else
            {
                if (annuleringLogic.AnnuleringID(id, email))
                    Console.WriteLine("Uw annulering wordt verwerkt.");
                isValid = true;
            }
        }
        while (!isValid);

        if (isValid)
        {
            Menu.Start();
        }
    }
}