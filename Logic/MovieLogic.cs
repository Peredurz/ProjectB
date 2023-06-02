using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class MovieLogic
{
    private static List<MovieModel> _movies;
    public const int RULE_LENGTH = 45;

    public MovieLogic()
    {
        _movies = MovieAccess.LoadAll();
    }

    // deze functie controleerd of de input van de user overeen komen met de movieID. 
    public bool MovieExist(int movieID)
    {
        foreach (MovieModel movie in _movies)
        {
            if (movieID == movie.ID)
            {
                return true;
            }
        }
        return false;
    }

    // Deze functie return de zaal nadat die is gecontroleerd door MovieExist. Als het false is return je 0
    public int GetAuditoriumID(int movieID)
    {
        bool movieExist = MovieExist(movieID);
        if (movieExist == true)
        {
            foreach (MovieModel movie in _movies)
            {
                if (movieID == movie.ID)
                {
                    return movie.AuditoriumID;
                }
            }
        }
        return 0;
    }

    public string ShowMovies()
    {
        string output = "";
        DateTime futureDate = new DateTime(1970, 1, 1);
        foreach (MovieModel movie in _movies)
        {
            if (movie.Time != futureDate)
            {
                foreach (var prop in movie.GetType().GetProperties())
                {
                    if (prop.Name == "AuditoriumID")
                    {
                        output += "Zaal" + ": " + prop.GetValue(movie) + Environment.NewLine;
                    }
                    else if (prop.Name == "Description")
                    {
                        continue;
                    }
                    else if (prop.Name == "AgeRestriction")
                    {
                        output += "Minimum Leeftijd" + ": " + prop.GetValue(movie) + " Jaar" + Environment.NewLine;
                    }
                    else if (prop.Name == "Time")
                    {
                        output += "Tijd" + ": " + prop.GetValue(movie) + Environment.NewLine;
                    }
                    else
                    {
                        output += prop.Name + ": " + prop.GetValue(movie) + Environment.NewLine;
                    }

                }
            }
            output += Environment.NewLine;
        }
        return output;
    }

    public string ShowFutureMovies()
    {
        string output = "";
        DateTime futureDate = new DateTime(1970, 1, 1);
        foreach (MovieModel movie in _movies)
        {
            if (movie.Time == futureDate)
            {
                foreach (var prop in movie.GetType().GetProperties())
                {
                    if (prop.Name == "Description")
                    {
                        continue;
                    }
                    else if (prop.Name == "AuditoriumID")
                    {
                        output += "Zaal" + ": " + prop.GetValue(movie) + Environment.NewLine;
                    }
                    else if (prop.Name == "Time")
                    {
                        output += "Tijd" + ": " + "Nog niet bekend" + Environment.NewLine;
                    }
                    else if (prop.Name == "AgeRestriction")
                    {
                        output += "Minimum Leeftijd" + ": " + prop.GetValue(movie) + " Jaar" + Environment.NewLine;
                    }
                    else
                    {
                        output += prop.Name + ": " + prop.GetValue(movie) + Environment.NewLine;
                    }

                }
                output += Environment.NewLine;
            }
        }
        return output;
    }

    /// <summary>
    /// Deze method zoekt een film op basis van de titel of de ID.
    /// En geeft dan een <see cref="MovieModel"/> terug.
    /// Als de ingevoerde string een ID is dan wordt deze geconverteerd naar een int met de functie <see cref="int.TryParse(string, out int)"/>
    /// Zo niet dan wordt er gekeken of de string een titel is bij <see cref="MovieLogic.GetMovieByTitle(string)"/>
    /// </summary>
    /// <param name="movie"></param>
    /// <returns><see cref="MovieModel"/></returns>
    public static MovieModel SearchMovie(string movie)
    {
        if (int.TryParse(movie, out int id))
        {
            return MovieLogic.GetMovie(Convert.ToInt32(id));
        }
        else if (movie.GetType() == typeof(string))
        {
            return MovieLogic.GetMovieByTitle(Convert.ToString(movie));
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Deze method controleerd of de datetime niet overlapt met een andere film.
    /// Dit wordt gebruikt als je de datum en tijd van een film wilt aanpassen.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="auditoriumID"></param>
    /// <param name="_movie"></param>
    /// <returns><see cref="bool"/></returns>
    public bool StartTimeInterference(DateTime time, int auditoriumID, MovieModel _movie)
    {
        foreach (MovieModel movie in _movies)
        {
            if (movie.AuditoriumID == auditoriumID && movie.ID != _movie.ID)
            {
                if (time >= movie.Time && time <= movie.Time.AddMinutes(movie.Duration))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Deze method controleerd of de nieuwe lengte van de film niet overlapt met een andere film.
    /// Het checkt of de minuten die erbij of eraf gaan niet overlappen met een andere film.
    /// </summary>
    /// <param name="minutes"></param>
    /// <param name="auditoriumID"></param>
    /// <param name="_movie"></param>
    /// <returns><see cref="bool"/></returns>
    public bool TimeInterference(int minutes, int auditoriumID, MovieModel _movie)
    {
        foreach (MovieModel movie in _movies)
        {
            if (movie.AuditoriumID == auditoriumID && movie.ID != _movie.ID)
            {
                DateTime endTime = _movie.Time.AddMinutes(minutes);
                if (movie.Time <= endTime && movie.Time.Date == endTime.Date)
                {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Deze method zoekt een film op basisi van de titel.
    /// En geeft dan een <see cref="MovieModel"/> terug.
    /// </summary>
    /// <param name="movie"></param>
    /// <returns><see cref="MovieModel"/></returns>
    public static MovieModel GetMovieByTitle(string movie) => _movies.Find(m => m.Title.ToLower() == movie.ToLower()) ?? null;

    public static MovieModel GetMovie(int movieID)
    {
        foreach (MovieModel _movie in _movies)
            if (_movie.ID == movieID)
                return _movie;
        return null;
    }

    public static void RemoveMovie(int movieID)
    {
        List<MovieModel> nonDeletedMovies = new List<MovieModel>();
        foreach (MovieModel movie in _movies)
        {
            if (movie.ID != movieID)
                nonDeletedMovies.Add(movie);
        }
        _movies = nonDeletedMovies;
        MovieAccess.WriteAll(_movies);
    }

    /// <summary>
    /// Method om een prive lijst van <see cref="MovieModel"/> te returnen. Omdat deze lijst privé is.
    /// </summary>
    /// <returns>List<see cref="MovieModel"/></returns>
    public static List<MovieModel> GetMovies() => _movies;

    public string ShowMovieDetails(int movieID)
    {
        MovieModel movie = MovieLogic.GetMovie(movieID);
        string output = "";
        foreach (var prop in movie.GetType().GetProperties())
        {
            if (prop.Name == "AuditoriumID")
            {
                output += "Zaal" + ": " + prop.GetValue(movie) + Environment.NewLine;
            }
            if (prop.Name == "Description")
            {
                int RULE_LENGTH = 45;
                string Description = prop.GetValue(movie).ToString();
                if (Description.Length > RULE_LENGTH)
                {
                    string[] DescriptionArray = Description.Split(' ');
                    string DescriptionOutput = "";
                    int DescriptionLength = 0;
                    foreach (string word in DescriptionArray)
                    {
                        DescriptionLength += word.Length;
                        if (DescriptionLength > RULE_LENGTH)
                        {
                            DescriptionOutput += Environment.NewLine;
                            DescriptionLength = 0;
                        }
                        DescriptionOutput += word + " ";
                    }
                    output += prop.Name + ": " + DescriptionOutput + Environment.NewLine;
                }
            }
            else
            {
                output += prop.Name + ": " + prop.GetValue(movie) + Environment.NewLine;
            }

        }
        output += Environment.NewLine;
        return output;
    }

    /// <summary>
    /// Deze method filtert de films op basis van de input van de gebruiker.
    /// 
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    public string FilterMovies(string userInput)
    {
        string filteredMovies = "";
        IEnumerable<MovieModel> movies;
        if (int.TryParse(userInput, out int duration))
        {
            // Als de gebruiker een getal invoert dan wordt er gekeken of het getal kleiner is dan 18 (leeftijdsgrens) 
            if (duration <= 18)
            {
                movies =
                from movie in _movies
                where movie.AgeRestriction <= duration
                select movie;
            }
            else
            {
                movies =
                from movie in _movies
                where movie.Duration == duration
                select movie;
            }

        }
        else if (DateTime.TryParse(userInput, out DateTime date))
        {
            movies =
            from movie in _movies
            where movie.Time.Date == date.Date
            select movie;
        }
        else
        {
            movies =
            from movie in _movies
            where movie.Genre.ToLower() == userInput.ToLower()
            select movie;
        }
        movies = movies.ToList();

        foreach (MovieModel movie in movies)
        {
            foreach (var prop in movie.GetType().GetProperties())
            {
                if (prop.Name == "AuditoriumID")
                {
                    filteredMovies += "Zaal" + ": " + prop.GetValue(movie) + Environment.NewLine;
                }
                else if (prop.Name == "Description")
                {
                    continue;
                }
                else
                {
                    filteredMovies += prop.Name + ": " + prop.GetValue(movie) + Environment.NewLine;
                }

            }
            filteredMovies += Environment.NewLine;
        }
        return filteredMovies;
    }
}