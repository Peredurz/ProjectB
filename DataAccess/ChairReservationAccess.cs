using System.Text.Json;

class ChairReservationAccess : AbstractAccess<ChairReservationModel>
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/chairreservation.json"));

    public override List<ChairReservationModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ChairReservationModel>>(json);
    }

    public override void WriteAll(List<ChairReservationModel> chairReservations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(chairReservations, options);
        File.WriteAllText(path, json);
    }
}