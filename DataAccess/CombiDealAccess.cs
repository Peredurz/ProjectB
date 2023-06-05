using System.Text.Json;
class CombiDealAccess : AbstractAccess<CombiDealModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/combideals.json"));
}
