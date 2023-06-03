using System.Text.Json;

/// <summary>
/// Dit is de class voor <c>AnnuleringAcces</c> om de annuleringen te kunnen toevoegen aan de json file.
/// En ook in te kunnen zien of zelfs te verwijderen
/// <para>
/// Dit gebeurt door de twee functies <see cref="AnnuleringAccess.LoadAll"/> en <see cref="AnnuleringAccess.WriteAll"/>.
/// <see cref="AnnuleringAccess.LoadAll"/> returns <returns>Lijst van <see cref="AnnuleringModel"/> objecten</returns>.
/// <see cref="AnnuleringAccess.WriteAll"/> kan je aanroepen door een lijst van <see cref="AnnuleringModel"/> objecten mee te geven.
/// </para>
/// </summary>
class AnnuleringAccess : AbstractAccess<AnnuleringModel>
{
    static readonly string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/annulering.json"));

    public override List<AnnuleringModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        if (json == "")
            return new List<AnnuleringModel>();
        return JsonSerializer.Deserialize<List<AnnuleringModel>>(json);
    }

    public override void WriteAll(List<AnnuleringModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}