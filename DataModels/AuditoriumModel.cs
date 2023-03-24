using System.Text.Json.Serialization;

class AuditoriumModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("chairs")]
    public List<int> Chairs { get; set; }

    [JsonPropertyName("totalchairs")]
    public int TotalChais { get; set; }

    [JsonPropertyName("totalrows")]
    public int TotalRows { get; set; }

    [JsonPropertyName("totalcols")]
    public int TotalCols { get; set; }

    public AuditoriumModel(int id, List<int> chairs, int totalChairs, int totalRows, int totalCols)
    {
        ID = id;
        Chairs = chairs;
        TotalChais = totalChairs;
        TotalRows = totalRows;
        TotalCols = totalCols;
    }
}