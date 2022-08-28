#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models.Comments;

public class Author
{
    [JsonPropertyName("user_no")] public int UserNo { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}

public class Item
{
    [JsonPropertyName("body")] public string Body { get; set; }
    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }
    [JsonPropertyName("comment_id")] public int CommentId { get; set; }
    [JsonPropertyName("author")] public Author Author { get; set; }
    [JsonPropertyName("latest_comment")] public List<LatestComment> LatestComment { get; set; }
}

public class LatestComment
{
    [JsonPropertyName("author")] public Author Author { get; set; }
    [JsonPropertyName("body")] public string Body { get; set; }
    [JsonPropertyName("comment_id")] public int CommentId { get; set; }
}

public class Comments
{
    [JsonPropertyName("items")] public List<Item> Items { get; set; }
}