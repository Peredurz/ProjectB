using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class MovieLogic
{
    private List<MovieModel> _movies;

    public MovieLogic()
    {
        _movies = MovieAccess.LoadAll();
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
                else
                {
                    output += prop.Name + ": " + prop.GetValue(movie) + Environment.NewLine;
                }
                
            }
            output += Environment.NewLine;
        }
        return output;
    }

}