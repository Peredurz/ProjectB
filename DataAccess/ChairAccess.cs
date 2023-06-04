using System.Text.Json;

class ChairAccess : AbstractAccess<ChairModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/chairs.json"));

    public override List<ChairModel> LoadAll()
    {

        string json = File.ReadAllText(path);
        if (json == "")
            return new List<ChairModel>();
        return JsonSerializer.Deserialize<List<ChairModel>>(json);
    }

    public override void WriteAll(List<ChairModel> chairs)
    {

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(chairs, options);
        File.WriteAllText(path, json);
    }
}