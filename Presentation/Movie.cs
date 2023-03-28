class Movie
{
    static private MovieLogic _movieLogic = new MovieLogic();

    public static string ShowMovies()
    {
        string movieOuput = Movie._movieLogic.ShowMovies();
        Console.WriteLine(movieOuput);
        return movieOuput;
    }
}