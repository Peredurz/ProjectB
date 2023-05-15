class Movie : IPresentation
{
    public static int AuditoriumID;
    public static int MovieID;
    static private MovieLogic _movieLogic = new MovieLogic();

    //Laat de gebruiker zien welke films er draaien. Laat de gebruiker kiezen welke film die wilt bekijken en print de zaal uit.  
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "movies";

        string movieOuput = Movie._movieLogic.ShowMovies();
        Console.WriteLine(movieOuput);
        PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
        string userOption = Console.ReadLine();
        if (int.TryParse(userOption, out _) == true)
        {
            ChooseMovie(Convert.ToInt32(userOption));
        }
        else if (userOption.ToLower() == "f")
        {
            string futureMovieOutput = _movieLogic.ShowFutureMovies();
            Console.WriteLine();
            Console.WriteLine(futureMovieOutput);
            PresentationLogic.WriteMenu(Menu.presentationModels, true);
            string userFutureOption = Console.ReadLine();
            if (int.TryParse(userFutureOption, out _) == true)
            {
                ChooseMovie(Convert.ToInt32(userFutureOption));
            }
            else if (userFutureOption.ToLower() == "b")
            {
                Movie.Start();
            }
            else
            {
                Console.WriteLine("Geen geldige invoer.");
                Movie.Start();
            }
        }
        else if (userOption.ToLower() == "b")
        {
            Menu.Start();
        }
        else
        {
            Console.WriteLine("Geen geldige invoer.");
            Movie.Start();
        }
    }

    public static void ChooseMovie(int userMovieID)
    {
        PresentationLogic.CurrentPresentation = "movie_submenu";
        int auditoriumID = Movie._movieLogic.GetAuditoriumID(userMovieID);
        if (auditoriumID != 0)
        {
            Movie.AuditoriumID = auditoriumID;
            Movie.MovieID = userMovieID;
            Console.WriteLine(Movie._movieLogic.ShowMovieDetails(userMovieID));
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
            string userInput = Console.ReadLine().ToLower();
            if (userInput == "b")
            {
                Movie.Start();
            }
            else if (userInput == "c")
            {
                Auditorium.Start();
            }
            else
            {
                Console.WriteLine("Geen correcte invoer");
                Movie.Start();
            }
        }
        else
        {
            Console.WriteLine("De film met dit ID bestaat niet");
            Movie.Start();
        }
    }
}