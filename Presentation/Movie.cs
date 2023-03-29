class Movie
{
    public static int AuditoriumID;
    public static int MovieID;
    static private MovieLogic _movieLogic = new MovieLogic();
    
    //Laat de gebruiker zien welke films er draaien. Laat de gebruiker kiezen welke film die wilt bekijken en print de zaal uit.  
    public static void Start()
    {
        string movieOuput = Movie._movieLogic.ShowMovies();
        Console.WriteLine(movieOuput);
        Console.Write(">");
        int userMovieID = Convert.ToInt32(Console.ReadLine());
        int auditoriumID = Movie._movieLogic.GetAuditoriumID(userMovieID);
        if (auditoriumID != 0)
        {
            Movie.AuditoriumID = auditoriumID;
            Movie.MovieID = userMovieID;
            Auditorium.Start();
        }
        else 
        {
            Console.WriteLine("The movieID does not exist");
        }
    }

}