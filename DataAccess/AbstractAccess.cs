using System.Text.Json;

public abstract class AbstractAccess<T>
{
    public abstract string path { get; }

    public List<T> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json);
    }

    public void WriteAll(List<T> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}