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
public class AccountsAccess : AbstractAccess<AccountModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));
}