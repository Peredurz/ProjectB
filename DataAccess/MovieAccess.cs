using System.Text.Json;

class MovieAccess : AbstractAccess<MovieModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/movies.json"));

    public override List<MovieModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        List<MovieModel> movieList = JsonSerializer.Deserialize<List<MovieModel>>(json)!;
        return movieList;
    }

    public override void WriteAll(List<MovieModel> movies)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(movies, options);
        File.WriteAllText(path, json);
    }
}