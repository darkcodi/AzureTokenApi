using System.Text.Json.Serialization;

namespace AzureTokenApi.Models;

public class DbTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_on")]
    public DateTimeOffset ExpiresOn { get; set; }

    [JsonPropertyName("resource")]
    public string Resource { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}