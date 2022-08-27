using System.Text.Json;
using System.Text.Json.Serialization;

namespace Band.Models;

public class GetFeed
{
    public FeedPayload FeedPayload { get; set; } = new();
    public AdPayload AdPayload { get; set; } = new();
}

public class FeedPayload
{
    [JsonPropertyName("feedVersion")] public string FeedVersion { get; set; } = "2.1.0";
    [JsonPropertyName("kidsYn")] public string KidsYn { get; set; } = "N";

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class AdPayload
{
    [JsonPropertyName("ad_token")] public string AdToken { get; set; } = "ygv_p3";
    [JsonPropertyName("os_type")] public string OsType { get; set; } = "pcweb";
    [JsonPropertyName("language")] public string Language { get; set; } = "en";
    [JsonPropertyName("agent_version")] public string AgentVersion { get; set; } = "7.4.0";
    [JsonPropertyName("country")] public string Country { get; set; } = "JP";
    [JsonPropertyName("placement")] public string Placement { get; set; } = "feed";

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}