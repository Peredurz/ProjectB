using System.Text.Json.Serialization;

public class ChairReservationModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("emailAdress")]
    public string EmailAdress { get; set; }

    [JsonPropertyName("chairId")]
    public int ChairID { get; set; }

    [JsonPropertyName("movieId")]
    public int MovieID { get; set; }

    [JsonPropertyName("auditoriumId")]
    public int AuditoriumID { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    private static int _idCounter = 0;

    [JsonPropertyName("totaalPrijs")]
    public double TotaalPrijs { get; set; }

    [JsonPropertyName("reserveringsCode")]
    public int ReserveringsCode { get; set; }

    public ChairReservationModel(string emailAdress, int chairId, int movieId ,int auditoriumId, DateTime time, double totaalPrijs, int reserveringsCode)
    {
        ID = ++_idCounter;
        EmailAdress = emailAdress;
        ChairID = chairId;
        MovieID = movieId;
        AuditoriumID = auditoriumId;
        Time = time;
        TotaalPrijs = totaalPrijs;
        ReserveringsCode = reserveringsCode;
    }
}