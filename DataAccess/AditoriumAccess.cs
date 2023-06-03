using System.Text.Json;

class AuditoriumAccess : AbstractAccess<AuditoriumModel>
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/auditoriums.json"));

    public override List<AuditoriumModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AuditoriumModel>>(json);
    }

    public override void WriteAll(List<AuditoriumModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}
