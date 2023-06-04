using System.Text.Json;

/// <summary>
/// Dit is de class voor <c>AnnuleringAcces</c> om de annuleringen te kunnen toevoegen aan de json file.
/// En ook in te kunnen zien of zelfs te verwijderen
/// <para>
/// Dit gebeurt door de twee functies <see cref="AccountsAccess.LoadAll"/> en <see cref="AccountsAccess.WriteAll"/>.
/// <see cref="AccountsAccess.LoadAll"/> returns <returns>Lijst van <see cref="AccountModel"/> objecten</returns>.
/// <see cref="AccountsAccess.WriteAll"/> kan je aanroepen door een lijst van <see cref="AccountModel"/> objecten mee te geven.
/// </para>
/// </summary>
class AccountsAccess : AbstractAccess<AccountModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));

    public override List<AccountModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AccountModel>>(json);
    }

    public override void WriteAll(List<AccountModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}