using System.Text.Json.Serialization;

public class AnnuleringModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("ReservationID")]
    public int ReservationID { get; set; }

    private static int _id = 0;
    public AnnuleringModel(string emailAddress, int reservationID)
    {
        ID = _id++;
        EmailAddress = emailAddress;
        ReservationID = reservationID;
    }
}