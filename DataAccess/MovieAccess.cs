using System.Text.Json;

class MovieAccess : AbstractAccess<MovieModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/movies.json"));
}