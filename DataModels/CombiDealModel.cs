using System.Text.Json.Serialization;

public class CombiDealModel
{
     [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("product")]
    public string Product { get; set; }
    [JsonPropertyName ("price")]
    public double Price { get; set;}

    private static int _idCounter = 0;
    
    public CombiDealModel(int id, string product, double price)

    {
        ID = ++_idCounter;
        Product = product;
        Price = price;
    }     
}