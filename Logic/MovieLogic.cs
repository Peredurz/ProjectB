using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class MovieLogic
{
    private List<MovieModel> _movies;
    public const int RULE_LENGTH = 45;

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

}