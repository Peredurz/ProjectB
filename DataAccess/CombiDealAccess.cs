using System.Text.Json;
class CombiDealAccess : AbstractAccess<CombiDealModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/combideals.json"));

    public override List<CombiDealModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<CombiDealModel>>(json);

        // StreamReader reader = new StreamReader(path);
        //string jsonString = reader.ReadToEnd();
        //reader.Close();
        // return JsonConvert.DeserializeObject<List<CombiDealModel>>(jsonString)!;
    }

    public override void WriteAll(List<CombiDealModel> combiDeals)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(combiDeals, options);
        File.WriteAllText(path, json);
    }
}
