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
                displayAnnuleringen(email);
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

    public static void displayAnnuleringen(string email)
    {
        List<ChairReservationModel> annulering = annuleringLogic.Annuleringen(email);
        List<MovieModel> movies = annuleringLogic.Movie();
        List<string> moviesList = new List<string>();

        foreach (ChairReservationModel reservation in annulering)
        {
            if (reservation.EmailAdress.ToLower() == email)
            {
                foreach (MovieModel movie in movies)
                {
                    if (movie.AuditoriumID == reservation.AuditoriumID && movie.Time == reservation.Time && movie.Time > DateTime.Now)
                    {
                        moviesList.Add($"\nID: {reservation.ID}\nFilm: {movie.Title}\nDatum en tijd: {reservation.Time}\nStoel: {reservation.ChairID}\n");
                    }
                }
            }
        }
        if (moviesList.Count == 0)
        {
            Console.WriteLine("U heeft geen geboekte films.\n");
            return;
        }

        bool isValid = false;
        do
        {
            Console.WriteLine("Dit zijn alle geboekte films: ");
            Console.WriteLine(string.Join("", moviesList));
            Console.WriteLine("Welke film wilt u annuleren? (ID)");
            Console.Write("> ");
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
        while (isValid == false);

        if (isValid == true)
        {
            PresentationLogic.CurrentPresentation = "menu";
            Menu.Start();
        }
    }
}