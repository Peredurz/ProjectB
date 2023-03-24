using System.Text.Json;

static class AuditoriumAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/auditoriums.json"));


    public static List<AuditoriumModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AuditoriumModel>>(json);
    }


    public static void WriteAll(List<AuditoriumModel> auditoriums)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(auditoriums, options);
        File.WriteAllText(path, json);
    }



}