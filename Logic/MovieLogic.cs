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
        foreach (MovieModel movie in _movies)
        {
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
        }
        return output;
    }

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

    public bool TimeInterference(int minutes, int auditoriumID, MovieModel _movie)
    {
        foreach (MovieModel movie in _movies)
        {
            if (movie.AuditoriumID == auditoriumID && movie.ID != _movie.ID)
            {
                if (movie.Time <= _movie.Time.AddMinutes(minutes))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static MovieModel GetMovieByTitle(string movie) => _movies.Find(m => m.Title.ToLower() == movie.ToLower());
    public static MovieModel GetMovie(int movieID) => _movies[movieID - 1];

    public static List<MovieModel> GetMovies() => _movies;
}