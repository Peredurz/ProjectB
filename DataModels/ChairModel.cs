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

    [JsonPropertyName("row")]
    public int Row { get; set; }

    [JsonPropertyName("column")]
    public int Column { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    private static int _idCounter = 0;

    public ChairModel(int row, int column, int price, string color, Status status)
    {
        Row = row;
        Column = column;
        Price = price;
        Color = color;
        ID = ++_idCounter;
        Status = status;
    }
}