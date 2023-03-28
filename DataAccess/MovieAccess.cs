using System.Text.Json;

static class MovieAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/movies.json"));

    public static List<MovieModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        List<MovieModel> movieList = JsonSerializer.Deserialize<List<MovieModel>>(json)!;
        return movieList;
    }

}