using System.Text.Json;

static class ChairAccess
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, "DataSources/chairs.json"));

    public static List<ChairModel> LoadAll()
    {
        try
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<ChairModel>>(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<ChairModel>();
        }
    }

    public static void WriteAll(List<ChairModel> chairs)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(chairs, options);
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}