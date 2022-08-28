#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models.Feed;

public class Author
{
    [JsonPropertyName("user_no")] public int UserNo { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}

public class Band
{
    [JsonPropertyName("band_no")] public int BandNo { get; set; }
}

public class Item
{
    [JsonPropertyName("post")] public Post Post { get; set; }
}

public class Post
{
    [JsonPropertyName("content")] public string Content { get; set; }
    [JsonPropertyName("band")] public Band Band { get; set; }
    [JsonPropertyName("post_no")] public int PostNo { get; set; }
    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }
    [JsonPropertyName("author")] public Author Author { get; set; }
}

public class Feed
{
    [JsonPropertyName("items")] public List<Item> Items { get; set; }
}