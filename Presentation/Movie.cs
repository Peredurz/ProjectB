public class Movie : IPresentation
{
    public static int AuditoriumID;
    public static int MovieID;
    static private MovieLogic _movieLogic = new MovieLogic();

    /// <summary>
    /// Laat de gebruiker zien welke films er draaien. Laat de gebruiker kiezen welke film die wilt bekijken en print de zaal uit. 
    /// </summary>
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "movies";

        string movieOuput = Movie._movieLogic.ShowMovies();
        Console.WriteLine(movieOuput);
        PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
        string userOption = PresentationLogic.GetUserInputFromMenu(true);
        if (userOption.ToLower() == "f")
        {
            PresentationLogic.CurrentPresentation = "moviesFuture";
            string futureMovieOutput = _movieLogic.ShowFutureMovies();
            bool loop = true;
            while (loop)
            {
                Console.WriteLine();
                Console.WriteLine(futureMovieOutput);
                Console.WriteLine("Omdat er geen datum bekend is kunnen deze films niet worden geboekt.");

                PresentationLogic.WriteMenu(Menu.presentationModels, true);
                string userFutureOption = Console.ReadLine();
                if (userFutureOption.ToLower() == "b")
                {
                    loop = false;
                    Movie.Start();
                    return;
                }
                else
                {
                    Console.WriteLine("Geen geldige invoer.");
                }
            }
        }
        else if (userOption.ToLower() == "a")
        {
            Movie.FilterMovie();
            return;
        }
        else if (userOption.ToLower() == "c")
        {
            ChooseMovie();
            return;
        }
        else if (userOption.ToLower() == "b")
        {
            Menu.Start();
            return;
        }
        else
        {
            Console.WriteLine("Geen geldige invoer.");
            Movie.Start();
            return;
        }
    }

    /// <summary>
    /// Laat de gebruiker een film kunnen kiezen.
    /// </summary>
    public static void ChooseMovie([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
        // Als de gebruiker niet vanuit de filter komt, laat dan alle films zien.
        if (memberName != "FilterMovie")
        {
            string movieOuput = Movie._movieLogic.ShowMovies();
            Console.WriteLine(movieOuput);
        }

        Console.WriteLine("Kies een film door het ID in te voeren.");
        Console.Write("> ");
        int choice = Int32.TryParse(Console.ReadLine(), out int userMovieID) ? userMovieID : -1;
        if (userMovieID == -1)
        {
            Console.WriteLine("Geen geldige invoer.");
            Movie.Start();
            return;
        }
        // when user enters an future movie id, show "geen geldige invoer"
        if (Movie._movieLogic.IsFutureMovie(userMovieID))
        {
            Console.WriteLine("Geen geldige invoer.");
            Movie.Start();
            return;
        }
        PresentationLogic.CurrentPresentation = "movie_submenu";
        int auditoriumID = Movie._movieLogic.GetAuditoriumID(userMovieID);
        if (auditoriumID != 0)
        {
            Movie.AuditoriumID = auditoriumID;
            Movie.MovieID = userMovieID;
            Console.WriteLine(Movie._movieLogic.ShowMovieDetails(userMovieID));
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
            Console.Write("> ");
            string userInput = Console.ReadLine().ToLower();
            if (userInput == "b")
            {
                Movie.Start();
                return;
            }
            else if (userInput == "c")
            {
                Auditorium.Start();
                return;
            }
            else
            {
                Console.WriteLine("Geen correcte invoer");
                Movie.Start();
                return;
            }
        }
        else
        {
            Console.WriteLine("De film met dit ID bestaat niet");
            PresentationLogic.CurrentMessage = "De film met dit ID bestaat niet.";
            Movie.Start();
            return;
        }
    }

    /// <summary>
    /// Om de gebruiker te kunnen vragen om te filteren op bepaalde attributen van een film.
    /// </summary>
    public static void FilterMovie()
    {
        Console.WriteLine("Geef iets wat je wilt filteren.(leeftijdgrens, genre, duur van de film, speeldatum(YYYY-MM-DD))");
        Console.Write("> ");
        string userInput = Console.ReadLine();
        string movieOutput = _movieLogic.FilterMovies(userInput);
        Console.WriteLine("==================================================");
        Console.WriteLine(movieOutput);
        if (movieOutput == "")
        {
            Console.WriteLine("Er zijn geen films gevonden met deze filter.");
            Console.WriteLine("Klik op een toets om terug te gaan naar het menu.");
            Console.ReadLine();
            Movie.Start();
            return;
        }
        else
        {
            Console.WriteLine("==================================================");
            Movie.ChooseMovie();
            return;
        }
    }
}