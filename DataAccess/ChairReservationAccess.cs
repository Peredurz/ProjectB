using System.Text.Json;

class ChairReservationAccess : AbstractAccess<ChairReservationModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/chairreservation.json"));

    public List<ChairReservationModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ChairReservationModel>>(json);
    }

    public void WriteAll(List<ChairReservationModel> chairReservations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(chairReservations, options);
        File.WriteAllText(path, json);
    }
}