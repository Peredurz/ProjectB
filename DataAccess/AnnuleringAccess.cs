using System.Text.Json;

static class AnnuleringAccess
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/annulering.json"));

    public static List<AnnuleringModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        if (json == "")
            return new List<AnnuleringModel>();
        return JsonSerializer.Deserialize<List<AnnuleringModel>>(json);
    }

    public static void WriteAll(List<AnnuleringModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}