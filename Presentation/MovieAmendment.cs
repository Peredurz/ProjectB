/// <summary>
/// Dit is de class <see cref="MovieAmendment"/>. Deze class zorgt ervoor dat de gebruiker ook wel een manager een film kan aanpassen.
/// Maar ook een film kan toevoegen of verwijderen.
/// De aanpassing van films is mogelijk gemaakt door de method <see cref="MovieAmendments(string)"/> waar een film wordt aangepast op basis van
/// een id of een titel.
/// </summary>
public class MovieAmendment : IPresentation
{
    private static MovieLogic _movieLogic = new MovieLogic();
    private static List<MovieModel> _movies = MovieLogic.GetMovies();

    /// <summary>
    /// Dit is de start method om deze class te starten. En uiteindelijk als gebruiker films aan te kunnen passen.
    /// Of een film toe te voegen of te verwijderen.
    /// De gebruiker kan uit de volgende opties kiezen:
    /// <list type="bullet">
    /// <item>
    /// <term>R</term>
    /// <description>Om een film te zoeken op basis van een id of een titel.</description>
    /// </item>
    /// <item>
    /// <term>A</term>
    /// <description>Om een film toe te voegen of te verwijderen.</description>
    /// </item>
    /// <item>
    /// <term>B</term>
    /// <description>Om terug te gaan naar het hoofdmenu.</description>
    /// </item>
    /// </list>
    /// </summary>
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "movie_editor";
        PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);


        string input = Console.ReadLine().ToLower();
        switch (input)
        {
            case "r":
                string movieOuput = _movieLogic.ShowMovies();
                Console.WriteLine(movieOuput);
                Console.WriteLine("Geef een film ID of een titel");
                Console.Write("> ");
                string inputUser = Console.ReadLine().ToLower();
                MovieAmendments(inputUser);
                break;
            case "a":
                MovieAdd();
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

    /// <summary>
    /// Deze method zorgt ervoor dat de gebruiker een film kan aanpassen op basis van een id of een titel.
    /// De gebruiker komt in een <see cref="do while"/> loop terecht waar de gebruiker de film kan aanpassen totdat de gebruiker tevreden is.
    /// Als bijvoorbeeld de tijd wordt aangepast wordt eerst gecheckt of de tijd niet overlapt met een andere film.
    /// Dit gebeurt ook als de duur van de film wordt aangepast.
    /// En als de gebruiker de zaal wilt veranderen van de film worden de twee voorgaand genoemde checks uitgevoerd.
    /// Hier krijgt de gebruiker de volgende opties om dingen aan te passen:
    /// <list type="bullet">
    /// <item>
    /// <term>A</term>
    /// <description>Om het zaal ID aan te passen.</description>
    /// </item>
    /// <item>
    /// <term>B</term>
    /// <description>Om de duur van de film aan te passen.</description>
    /// </item>
    /// <item>
    /// <term>C</term>
    /// <description>Om de start tijd van de film aan te passen.</description>
    /// </item>
    /// <item>
    /// <term>D</term>
    /// <description>Om de beschrijving van de film aan te passen.</description>
    /// </item>
    /// <item>
    /// <term>T</term>
    /// <description>Om de titel van de film aan te passen.</description>
    /// </item>
    /// <item>
    /// <term>E</term>
    /// <description>Om terug te gaan naar het hoofdmenu.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="void"/></returns>
    public static void MovieAmendments(string input)
    {
        MovieModel movie = MovieLogic.SearchMovie(input);
        if (movie == null)
        {
            Console.WriteLine("Deze film bestaat niet.");
            MovieAmendment.Start();
        }
        Console.WriteLine($"Wilt u de film {movie.Title} aanpassen? (Y/N)");
        Console.Write("> ");
        string choice = Console.ReadLine().ToLower();
        if (choice == "y")
        {
            bool loop = true;
            while (loop)
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
                            break;
                        }
                        if (_movieLogic.StartTimeInterference(movie.Time, auditoriumID, movie) || _movieLogic.TimeInterference(movie.Duration, auditoriumID, movie))
                        {
                            Console.WriteLine("Deze tijd is al bezet.");
                            break;
                        }
                        movie.AuditoriumID = auditoriumID;
                        Console.WriteLine("De nieuwe zaal ID van de film is: " + movie.AuditoriumID);
                        break;
                    case "b":
                        Console.WriteLine("Geef de nieuwe tijd van hoe lang de film duurt in minuten.");
                        Console.Write("> ");
                        int duration = int.TryParse(Console.ReadLine(), out duration) ? duration : -1;
                        if (duration < 0)
                        {
                            Console.WriteLine("Verkeerde invoer");
                            break;
                        }
                        if (_movieLogic.TimeInterference(duration, movie.AuditoriumID, movie))
                        {
                            Console.WriteLine("Deze eindtijd is al bezet door een andere film.");
                            break;
                        }
                        movie.Duration = duration;
                        Console.WriteLine("De nieuwe duur van de film is: " + movie.Duration);
                        break;
                    case "c":
                        Console.WriteLine("Geef de nieuwe tijd en datum voor de nieuwe start van de film begint. (YYYY-MM-DDTHH:MM:SS 0000-00-00T00:00:00)");
                        Console.Write("> ");
                        DateTime time = DateTime.TryParse(Console.ReadLine(), out time) ? time : DateTime.Now;
                        if (time < DateTime.Now)
                        {
                            Console.WriteLine("Verkeerde invoer");
                            break;
                        }
                        if (_movieLogic.StartTimeInterference(time, movie.AuditoriumID, movie))
                        {
                            Console.WriteLine("Deze tijd is al bezet.");
                            break;
                        }
                        movie.Time = time;
                        Console.WriteLine("De nieuwe start tijd van de film is: " + movie.Time);
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
                        loop = false;
                        continue;
                    default:
                        loop = false;
                        Console.WriteLine("Verkeerde invoer.");
                        continue;
                }
                _movies[movie.ID - 1] = movie;
                MovieAccess.WriteAll(_movies);
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
                    continue;
                }
                else
                {
                    loop = false;
                    Console.WriteLine("Verkeerde invoer.");
                    continue;
                }
            }
            MovieAmendment.Start();
        }
        else if (choice == "n")
        {
            MovieAmendment.Start();
        }
        else
        {
            Console.WriteLine("Verkeerde invoer.");
            MovieAmendment.Start();
        }
    }

    public static void MovieAdd()
    {
        Console.WriteLine("Wilt u een nieuwe film toevoegen of een film dat al draait op een andere tijd/zaal overkopiëren? (N/O)");
        Console.WriteLine("N: Nieuwe film toevoegen");
        Console.WriteLine("O: Overkopiëren");
        Console.WriteLine("B: Terug");
        Console.Write("> ");
        string inputUser = Console.ReadLine().ToLower();
        switch (inputUser)
        {
            case "n":
                NewMovie();
                break;
            case "o":
                // De volgende twee regels zijn nodig om de films te laten zien. Maar dit laad ook de films opnieuw in.
                // Waardoor het in PresentationLogic bij GetMovieByTitle een lijst heeft waar doorheen gezocht kan worden.
                // Als dit wordt weggehaald werkt het niet meer.
                string movieOuput = _movieLogic.ShowMovies();
                Console.WriteLine(movieOuput);
                CopyMovie();
                break;
            case "b":
                MovieAmendment.Start();
                break;
            default:
                Console.WriteLine("Verkeerde invoer.");
                MovieAmendment.Start();
                break;
        }
    }

    public static void CopyMovie()
    {
        Console.WriteLine("Geef de titel of het ID van de film die u wilt overkopiëren naar een nieuwe datum en tijd.");
        string input = Console.ReadLine().ToLower();
        MovieModel movie = MovieLogic.SearchMovie(input);
        if (movie == null)
        {
            Console.WriteLine("Deze film bestaat niet.");
            MovieAdd();
        }
        Console.WriteLine($"Wilt u de film {movie.Title} overkopiëren? (Y/N)");
        Console.Write("> ");
        string choice = Console.ReadLine().ToLower();
        if (choice == "y")
        {
            Console.WriteLine("Geef de nieuwe datum en tijd voor de nieuwe start van de film begint. (YYYY-MM-DDTHH:MM:SS 0000-00-00T00:00:00)");
            Console.Write("> ");
            DateTime time = DateTime.TryParse(Console.ReadLine(), out time) ? time : DateTime.Now;
            if (time < DateTime.Now)
            {
                Console.WriteLine("Verkeerde invoer");
                MovieAmendment.Start();
            }

            Console.WriteLine("Geef het zaal ID waarin de film draait.");
            Console.Write("> ");
            int auditoriumID = int.TryParse(Console.ReadLine(), out auditoriumID) ? auditoriumID : 0;
            MovieModel Movie = new MovieModel(_movies.Count + 1, auditoriumID, movie.Title, movie.Description, time, movie.Duration);
            if (_movieLogic.StartTimeInterference(time, auditoriumID, Movie) || _movieLogic.TimeInterference(movie.Duration, auditoriumID, Movie))
            {
                Console.WriteLine("Deze tijd is al bezet.");
                MovieAmendment.Start();
            }
            if (auditoriumID == 0)
            {
                Console.WriteLine("Zaal bestaat niet.");
                MovieAmendment.Start();
            }

            Console.WriteLine("Wilt u de film toevoegen? (Y/N)");
            Console.Write("> ");
            string inputUser2 = Console.ReadLine().ToLower();
            if (inputUser2 == "y")
            {
                _movies.Add(Movie);
                MovieAccess.WriteAll(_movies);
                Console.WriteLine("De film is toegevoegd.");
                MovieAmendment.Start();
            }
            else if (inputUser2 == "n")
            {
                Console.WriteLine("De film is niet toegevoegd.");
                MovieAmendment.Start();
            }
            else
            {
                Console.WriteLine("Verkeerde invoer.");
                MovieAmendment.Start();
            }
        }
        else if (choice == "n")
        {
            MovieAmendment.Start();
        }
    }

    public static void NewMovie()
    {
        Console.WriteLine("Geef de titel van de film.");
        Console.Write("> ");
        string title = Console.ReadLine();

        Console.WriteLine("Geef een beschrijving van de film.");
        Console.Write("> ");
        string description = Console.ReadLine();

        // bool correct = false;
        // DateTime dateTime;
        // do
        // {
        //     Console.WriteLine("Geef de datum van de dag waarop deze film in de bioscoop gaat draaien in dit formaat: YYYY-MM-DD.");
        //     Console.Write("> ");
        //     string date = Console.ReadLine();
        //     Console.WriteLine("Geef de tijd waarop de film draait in het volgende formaat: HH:MM:SS");
        //     Console.Write("> ");
        //     string time = Console.ReadLine();
        //     string dateTimeString = $"{date}T{time}";
        //     dateTime = DateTime.TryParse(dateTimeString, out dateTime) ? dateTime : DateTime.Now;
        //     if (dateTime > DateTime.Now)
        //     {
        //         correct = true;
        //     }
        // } while (!correct);

        int duration;
        do
        {
            Console.WriteLine("Geef de duur van de film in minuten.");
            Console.Write("> ");
            duration = int.TryParse(Console.ReadLine(), out duration) ? duration : -1;
        } while (duration <= 0);

        Console.WriteLine("Geef het zaal ID waarin de film draait.");
        Console.Write("> ");
        int auditoriumID = int.TryParse(Console.ReadLine(), out auditoriumID) ? auditoriumID : 0;
        MovieModel Movie = new MovieModel(_movies.Count + 1, auditoriumID, title, description, new DateTime(1970, 1, 1), duration);
        Console.WriteLine("De datum en tijd waarop de film draait moet u later nog zetten met de functie om een film aan te passen.");
        Console.WriteLine("Wilt u de film toevoegen? (Y/N)");
        Console.Write("> ");
        string inputUser = Console.ReadLine().ToLower();
        if (inputUser == "y")
        {
            _movies.Add(Movie);
            MovieAccess.WriteAll(_movies);
            Console.WriteLine("De film is toegevoegd.");
            MovieAmendment.Start();
        }
        else if (inputUser == "n")
        {
            Console.WriteLine("De film is niet toegevoegd.");
            MovieAmendment.Start();
        }
        else
        {
            Console.WriteLine("Verkeerde invoer.");
            MovieAmendment.Start();
        }
    }
}