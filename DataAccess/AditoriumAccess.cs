using System.Text.Json;

class AuditoriumAccess : AbstractAccess<AuditoriumModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/auditoriums.json"));
}
