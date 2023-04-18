using System.Text.Json.Serialization;

class ChairReservationModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("emailAdress")]
    public string EmailAdress { get; set; }

    [JsonPropertyName("chairId")]
    public int ChairID { get; set; }

    [JsonPropertyName("auditoriumId")]
    public int AuditoriumID { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    private static int _idCounter = 0;

    public ChairReservationModel(string emailAdress, int chairId, int auditoriumId, DateTime time)
    {
        EmailAdress = emailAdress;
        ChairID = chairId;
        AuditoriumID = auditoriumId;
        Time = time;
    }
}