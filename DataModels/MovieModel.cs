using System.Text.Json.Serialization;


public class MovieModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("auditorium_id")]
    public int AuditoriumID { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("genre")]
    public string Genre { get; set; }

    [JsonPropertyName("age_restriction")]
    public int AgeRestriction { get; set; }
    public MovieModel(int id, int auditoriumID, string title, string description, DateTime time, int duration, string genre, int ageRestriction)
    {
        ID = id;
        AuditoriumID = auditoriumID;
        Title = title;
        Description = description;
        Time = time;
        Duration = duration;
        Genre = genre;
        AgeRestriction = ageRestriction;
    }

}
