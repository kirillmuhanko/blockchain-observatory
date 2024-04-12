using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WorkerService.Models;

public class UserJsonModel
{
    [JsonPropertyName("chatId")]
    [JsonProperty(Required = Required.Always)]
    public long ChatId { get; set; }

    [JsonPropertyName("userId")]
    [JsonProperty(Required = Required.Always)]
    public long UserId { get; set; }

    [JsonPropertyName("username")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Username { get; set; }

    [JsonPropertyName("firstName")]
    [JsonProperty(Required = Required.Always)]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? LastName { get; set; }

    [JsonPropertyName("createdAt")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public DateTime? CreatedAt { get; set; }
}