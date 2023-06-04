using System.Text.Json;

class ChairAccess : AbstractAccess<ChairModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/chairs.json"));
}