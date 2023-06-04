using System.Text.Json;

class ChairReservationAccess : AbstractAccess<ChairReservationModel>
{
    public override string path { get; } = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/chairreservation.json"));
}