class Movie : IPresentation
{
    public static int AuditoriumID;
    public static int MovieID;
    static private MovieLogic _movieLogic = new MovieLogic();

    //Laat de gebruiker zien welke films er draaien. Laat de gebruiker kiezen welke film die wilt bekijken en print de zaal uit.  
    public static void Start()
    {
        string movieOuput = Movie._movieLogic.ShowMovies();
        Console.WriteLine(movieOuput);
        PresentationLogic.WriteMenu(Menu.presentationModels, true);
        string userOption = Console.ReadLine();
        if (int.TryParse(userOption, out _) == true)
        {
            ChooseMovie(Convert.ToInt32(userOption));
        }
        else if (userOption.ToLower() == "b")
        {
            PresentationLogic.CurrentPresentation = "menu";
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
        int auditoriumID = Movie._movieLogic.GetAuditoriumID(userMovieID);
        if (auditoriumID != 0)
        {
            PresentationLogic.CurrentPresentation = "auditorium";
            Movie.AuditoriumID = auditoriumID;
            Movie.MovieID = userMovieID;
            Auditorium.Start();
        }
        else
        {
            PresentationLogic.CurrentPresentation = "menu";
            Console.WriteLine("De film met dit ID bestaat niet");
            return;
        }
    }
}