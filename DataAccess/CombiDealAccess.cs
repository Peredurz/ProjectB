using System.Text.Json;
public class CombiDealAccess
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/combideals.json"));

    public static List<CombiDealModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<CombiDealModel>>(json);

        // StreamReader reader = new StreamReader(path);
        //string jsonString = reader.ReadToEnd();
        //reader.Close();
        // return JsonConvert.DeserializeObject<List<CombiDealModel>>(jsonString)!;
    }

    public static void WriteAll(List<CombiDealModel> combiDeals)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(combiDeals, options);
        File.WriteAllText(path, json);
    }
}
