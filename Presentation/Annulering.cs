class Annulering
{
    private static AnnuleringLogic annuleringLogic = new AnnuleringLogic();

    // Lijst met alle access
    private static readonly List<string> _allClearance = new List<string>() { "Manager", "Worker", "Customer" };

    // Lijst met alle presentatiemodellen die nodig zijn voor annuleringen aanvragen
    private static readonly List<PresentationModel> _presentationModels = new()
    {
        new PresentationModel("R", "Annulering", _allClearance, true),
        new PresentationModel("B", "Terug", _allClearance, true),
    };
    public static void Start()
    {
        PresentationLogic.WriteMenu(_presentationModels, true);

        bool loop = true;
        while (loop)
        {
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
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Verkeerde invoer");
                    break;
            }
            PresentationLogic.WriteMenu(_presentationModels, true);
        }
        Menu.Start();
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

        Console.WriteLine("Dit zijn alle geboekte films: ");
        Console.WriteLine(string.Join("", moviesList));
        Console.WriteLine("Welke film wilt u annuleren? (ID)");
        Console.Write("> ");
        int id = Int32.TryParse(Console.ReadLine(), out id) ? id : -1;
        if (id < 0)
        {
            Console.WriteLine("Verkeerde invoer");
            return;
        }
        else
        {
            if (annuleringLogic.AnnuleringID(id, email))
                Console.WriteLine("Uw annulering wordt verwerkt.");
        }
    }
}