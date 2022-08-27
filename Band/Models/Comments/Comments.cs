#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models.Comments;

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

public class FirstParams
{
    [JsonPropertyName("resolution_type")] public string ResolutionType { get; set; }

    [JsonPropertyName("paging_default")] public string PagingDefault { get; set; }

    [JsonPropertyName("params_for_first")] public string ParamsForFirst { get; set; }

    [JsonPropertyName("limit")] public string Limit { get; set; }

    [JsonPropertyName("update_param")] public string UpdateParam { get; set; }

    [JsonPropertyName("content_key")] public string ContentKey { get; set; }

    [JsonPropertyName("band_no")] public string BandNo { get; set; }

    [JsonPropertyName("is_last_paging_value_for_included")]
    public string IsLastPagingValueForIncluded { get; set; }

    [JsonPropertyName("last_paging_value")]
    public string LastPagingValue { get; set; }
}

public class Item
{
    [JsonPropertyName("body")] public string Body { get; set; }

    [JsonPropertyName("is_translatable")] public bool IsTranslatable { get; set; }

    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }

    [JsonPropertyName("is_secret")] public bool IsSecret { get; set; }

    [JsonPropertyName("comment_id")] public int CommentId { get; set; }

    [JsonPropertyName("written_in")] public string WrittenIn { get; set; }

    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("content_type")] public string ContentType { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("audio_duration")] public int AudioDuration { get; set; }

    [JsonPropertyName("post_comment_id")] public int PostCommentId { get; set; }

    [JsonPropertyName("is_liked_by_viewer")]
    public bool IsLikedByViewer { get; set; }

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

    [JsonPropertyName("origin_comment_id")]
    public int OriginCommentId { get; set; }

    [JsonPropertyName("comment_id")] public int CommentId { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("common_emotion_type")]
    public List<string> CommonEmotionType { get; set; }

    [JsonPropertyName("is_secret")] public bool IsSecret { get; set; }

    [JsonPropertyName("audio_duration")] public int AudioDuration { get; set; }

    [JsonPropertyName("is_visible_only_to_author")]
    public bool IsVisibleOnlyToAuthor { get; set; }
}

public class Paging
{
    [JsonPropertyName("previous_params")] public object PreviousParams { get; set; }

    [JsonPropertyName("next_params")] public object NextParams { get; set; }

    [JsonPropertyName("first_params")] public FirstParams FirstParams { get; set; }

    [JsonPropertyName("last_params")] public object LastParams { get; set; }
}

public class Comments
{
    [JsonPropertyName("items")] public List<Item> Items { get; set; }

    [JsonPropertyName("paging")] public Paging Paging { get; set; }

    [JsonPropertyName("total")] public int Total { get; set; }

    [JsonPropertyName("update_param")] public UpdateParam UpdateParam { get; set; }
}

public class UpdateParam
{
    [JsonPropertyName("resolution_type")] public string ResolutionType { get; set; }

    [JsonPropertyName("content_key")] public string ContentKey { get; set; }

    [JsonPropertyName("band_no")] public string BandNo { get; set; }

    [JsonPropertyName("last_paging_value")]
    public int LastPagingValue { get; set; }
}