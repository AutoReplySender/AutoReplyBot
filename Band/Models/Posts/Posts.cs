#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models.Posts;

public class Attachment
{
    [JsonPropertyName("video")] public List<object> Video { get; set; }

    [JsonPropertyName("photo")] public List<Photo> Photo { get; set; }

    [JsonPropertyName("file")] public List<object> File { get; set; }

    [JsonPropertyName("dropbox_file")] public List<object> DropboxFile { get; set; }

    [JsonPropertyName("external_file")] public List<object> ExternalFile { get; set; }
}

public class Author
{
    [JsonPropertyName("user_no")] public int UserNo { get; set; }

    [JsonPropertyName("band_no")] public int BandNo { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("member_type")] public string MemberType { get; set; }

    [JsonPropertyName("member_certified")] public bool MemberCertified { get; set; }

    [JsonPropertyName("me")] public bool Me { get; set; }

    [JsonPropertyName("is_muted")] public bool IsMuted { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }
}

public class Band
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("is_recruiting")] public bool IsRecruiting { get; set; }

    [JsonPropertyName("band_no")] public int BandNo { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("cover")] public string Cover { get; set; }

    [JsonPropertyName("theme_color")] public string ThemeColor { get; set; }

    [JsonPropertyName("available_actions")]
    public List<string> AvailableActions { get; set; }

    [JsonPropertyName("permissions")] public List<string> Permissions { get; set; }
}

public class FeaturedComment
{
    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("body")] public string Body { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("band_no")] public int BandNo { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("content_type")] public string ContentType { get; set; }

    [JsonPropertyName("comment_id")] public int CommentId { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("origin_comment_id")]
    public int? OriginCommentId { get; set; }
}

public class FirstParams
{
    [JsonPropertyName("resolution_type")] public string ResolutionType { get; set; }

    [JsonPropertyName("limit")] public string Limit { get; set; }

    [JsonPropertyName("band_no")] public string BandNo { get; set; }

    [JsonPropertyName("direction")] public string Direction { get; set; }
}

public class Item
{
    [JsonPropertyName("attachment")] public Attachment Attachment { get; set; }

    [JsonPropertyName("featured_comment")] public List<FeaturedComment> FeaturedComment { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }

    [JsonPropertyName("photo_count")] public int PhotoCount { get; set; }

    [JsonPropertyName("band")] public Band Band { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("written_in")] public string WrittenIn { get; set; }

    [JsonPropertyName("content_lineage")] public string ContentLineage { get; set; }

    [JsonPropertyName("shared_count")] public int SharedCount { get; set; }

    [JsonPropertyName("web_url")] public string WebUrl { get; set; }

    [JsonPropertyName("is_bookmarked")] public bool IsBookmarked { get; set; }

    [JsonPropertyName("read_count")] public int ReadCount { get; set; }

    [JsonPropertyName("should_disable_comment")]
    public bool ShouldDisableComment { get; set; }

    [JsonPropertyName("copiable_state")] public string CopiableState { get; set; }

    [JsonPropertyName("is_translatable")] public bool IsTranslatable { get; set; }

    [JsonPropertyName("video_count")] public int VideoCount { get; set; }

    [JsonPropertyName("is_major_notice")] public bool IsMajorNotice { get; set; }

    [JsonPropertyName("common_emotion_type")]
    public List<string> CommonEmotionType { get; set; }

    [JsonPropertyName("latest_comment")] public List<LatestComment> LatestComment { get; set; }
}

public class LatestComment
{
    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("body")] public string Body { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("band_no")] public int BandNo { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("content_type")] public string ContentType { get; set; }

    [JsonPropertyName("comment_id")] public int CommentId { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }
}

public class NextParams
{
    [JsonPropertyName("resolution_type")] public string ResolutionType { get; set; }

    [JsonPropertyName("limit")] public string Limit { get; set; }

    [JsonPropertyName("after")] public string After { get; set; }

    [JsonPropertyName("band_no")] public string BandNo { get; set; }

    [JsonPropertyName("direction")] public string Direction { get; set; }
}

public class Paging
{
    [JsonPropertyName("previous_params")] public object PreviousParams { get; set; }

    [JsonPropertyName("next_params")] public NextParams NextParams { get; set; }

    [JsonPropertyName("first_params")] public FirstParams FirstParams { get; set; }

    [JsonPropertyName("last_params")] public object LastParams { get; set; }
}

public class Photo
{
    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("post_id")] public string PostId { get; set; }

    [JsonPropertyName("photo_id")] public string PhotoId { get; set; }

    [JsonPropertyName("height")] public int Height { get; set; }

    [JsonPropertyName("width")] public int Width { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("registered_at")] public string RegisteredAt { get; set; }

    [JsonPropertyName("photo_no")] public object PhotoNo { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("savable_state")] public string SavableState { get; set; }

    [JsonPropertyName("photo_url")] public string PhotoUrl { get; set; }

    [JsonPropertyName("photo_thumbnail")] public string PhotoThumbnail { get; set; }

    [JsonPropertyName("original_post_id")] public string OriginalPostId { get; set; }

    [JsonPropertyName("source")] public string Source { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("content_type")] public string ContentType { get; set; }
}

public class Posts
{
    [JsonPropertyName("paging")] public Paging Paging { get; set; }

    [JsonPropertyName("items")] public List<Item> Items { get; set; }
}