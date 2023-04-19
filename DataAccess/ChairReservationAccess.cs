using System.Text.Json;

static class ChairReservationAccess
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));

    public static List<ChairReservationModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ChairReservationModel>>(json);
    }

    public static void WriteAll(List<ChairReservationModel> chairReservations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(chairReservations, options);
        File.WriteAllText(path, json);
    }
}