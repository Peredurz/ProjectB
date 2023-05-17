public class MovieAmendment : IPresentation
{
    private static MovieLogic _movieLogic = new MovieLogic();
    private static List<MovieModel> _movies = MovieLogic.GetMovies();
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "movie_editor";
        PresentationLogic.WriteMenu(Menu.presentationModels, true);


        string input = Console.ReadLine().ToLower();
        switch (input)
        {
            case "r":
                string movieOuput = _movieLogic.ShowMovies();
                Console.WriteLine(movieOuput);
                Console.WriteLine("Geef een film ID of een titel");
                Console.Write("> ");
                string inputUser = Console.ReadLine().ToLower();
                movieAmendment(inputUser);
                break;
            case "a":
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

    public static void movieAmendment(string input)
    {
        MovieModel movie = MovieLogic.SearchMovie(input);
        Console.WriteLine($"Wilt u de film {movie.Title} aanpassen? (Y/N)");
        Console.Write("> ");
        string choice = Console.ReadLine().ToLower();
        if (choice == "y")
        {
            bool loop = true;
            do
            {
                Console.WriteLine("Wat wilt u aanpassen aan de film?");
                Console.WriteLine("A: AuditoriumID");
                Console.WriteLine("B: tijd van hoe lang de fim duurt");
                Console.WriteLine("C: tijd van wanneer de film begint");
                Console.WriteLine("D: Beschrijving");
                Console.WriteLine("T: Titel");
                Console.WriteLine("E: Exit");
                Console.Write("> ");
                string inputUser = Console.ReadLine().ToLower();
                switch (inputUser)
                {
                    case "a":
                        Console.WriteLine("Geef het nieuwe zaal ID");
                        Console.Write("> ");
                        int auditoriumID = int.TryParse(Console.ReadLine(), out auditoriumID) ? auditoriumID : -1;
                        if (auditoriumID < 0)
                        {
                            Console.WriteLine("Verkeerde invoer");
                            MovieAmendment.Start();
                        }
                        movie.AuditoriumID = auditoriumID;
                        break;
                    case "b":
                        Console.WriteLine("Geef de nieuwe tijd van hoe lang de film duurt in minuten.");
                        Console.Write("> ");
                        int duration = int.TryParse(Console.ReadLine(), out duration) ? duration : -1;
                        if (duration < 0)
                        {
                            Console.WriteLine("Verkeerde invoer");
                            MovieAmendment.Start();
                        }
                        movie.Duration = duration;
                        break;
                    case "c":
                        Console.WriteLine("Geef de nieuwe tijd van wanneer de film begint.");
                        Console.Write("> ");
                        DateTime time = DateTime.TryParse(Console.ReadLine(), out time) ? time : DateTime.Now;
                        if (time < DateTime.Now)
                        {
                            Console.WriteLine("Verkeerde invoer");
                            MovieAmendment.Start();
                        }
                        movie.Time = time;
                        break;
                    case "d":
                        Console.WriteLine("Geef de nieuwe beschrijving van de film.");
                        Console.Write("> ");
                        string description = Console.ReadLine();
                        movie.Description = description;
                        break;
                    case "t":
                        Console.WriteLine("Geef de nieuwe titel van de film.");
                        Console.Write("> ");
                        string title = Console.ReadLine();
                        movie.Title = title;
                        break;
                    case "e":
                        MovieAmendment.Start();
                        break;
                    default:
                        Console.WriteLine("Verkeerde invoer.");
                        MovieAmendment.Start();
                        break;
                }
                _movies[movie.ID - 1] = movie;
                MovieAccess.WriteAll(_movies);
                Console.WriteLine("De film is aangepast.");
                Console.WriteLine("Wilt u nog andere dingen van de huidige gekozen film aanpassen? (Y/N)");
                Console.Write("> ");
                string inputUser2 = Console.ReadLine().ToLower();
                if (inputUser2 == "y")
                {
                    continue;
                }
                else if (inputUser2 == "n")
                {
                    loop = false;
                    MovieAmendment.Start();
                }
                else
                {
                    Console.WriteLine("Verkeerde invoer.");
                    MovieAmendment.Start();
                }
            } while (loop);
        }
        else if (choice == "n")
        {
            MovieAmendment.Start();
        }
        else
        {
            Console.WriteLine("Verkeerde invoer.");
            Start();
        }
    }
}