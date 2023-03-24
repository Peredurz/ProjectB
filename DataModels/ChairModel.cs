using System.Text.Json.Serialization;
public enum Status
{
    Available,
    Pending,
    Reserved
}

public class ChairModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    private int _id;

    [JsonPropertyName("row")]
    public int Row { get; set; }

    [JsonPropertyName("column")]
    public int Column { get; set; }

    [JsonPropertyName("price")]
    public double price { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    public ChairModel(int _Row, int _Column, int _Price, string _Color)
    {
        Row = _Row;
        Column = _Column;
        price = _Price;
        Color = _Color;
        ID = ++_id;
        Status = Status.Available;
    }
}