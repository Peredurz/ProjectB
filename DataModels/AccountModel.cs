using System.Text.Json.Serialization;

class AccountModel
{

    [JsonPropertyName("id")]
    public int Id { get; set; } = 2;

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("userType")]
    public string UserType { get; set; }

    public AccountModel(string emailAddress, string password, string fullName, string userType)
    {
        Id++;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        UserType = userType;
    }
}
