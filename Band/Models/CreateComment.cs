using System.Text.Json;
using System.Text.Json.Serialization;

namespace Band.Models;

public class CreateComment
{
    public required int BandNo { get; set; }
    public string MemberType { get; set; } = "membership";
    public required string Body { get; set; }
    public string Photos { get; set; } = "";
    public string File { get; set; } = "";
    public string Video { get; set; } = "";
    public required ContentKey ContentKey { get; set; }
}

public class ContentKey
{
    [JsonPropertyName("content_type")] public string ContentType { get; set; } = "post";
    [JsonPropertyName("post_no")] public required int PostNo { get; set; }
    [JsonPropertyName("comment_id")] public int? CommentId { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}