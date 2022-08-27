#nullable disable
using System.Text.Json.Serialization;

namespace Band.Models.Feed;

public class Attachment
{
    [JsonPropertyName("video")] public List<Video> Video { get; set; }

    [JsonPropertyName("photo")] public List<Photo> Photo { get; set; }

    [JsonPropertyName("file")] public List<object> File { get; set; }

    [JsonPropertyName("dropbox_file")] public List<object> DropboxFile { get; set; }

    [JsonPropertyName("external_file")] public List<object> ExternalFile { get; set; }

    [JsonPropertyName("snippet")] public List<Snippet> Snippet { get; set; }
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

    [JsonPropertyName("is_deleted")] public bool? IsDeleted { get; set; }
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

public class Item
{
    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("post")] public Post Post { get; set; }

    [JsonPropertyName("content_lineage")] public string ContentLineage { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("is_recommended_feed")]
    public bool IsRecommendedFeed { get; set; }
}

public class NextParams
{
    [JsonPropertyName("ad_payload")] public string AdPayload { get; set; }

    [JsonPropertyName("update_param")] public string UpdateParam { get; set; }

    [JsonPropertyName("feed_next_param")] public string FeedNextParam { get; set; }

    [JsonPropertyName("feed_payload")] public string FeedPayload { get; set; }
}

public class Paging
{
    [JsonPropertyName("previous_params")] public object PreviousParams { get; set; }

    [JsonPropertyName("next_params")] public NextParams NextParams { get; set; }
}

public class Photo
{
    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("photo_id")] public string PhotoId { get; set; }

    [JsonPropertyName("post_id")] public string PostId { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("height")] public int Height { get; set; }

    [JsonPropertyName("width")] public int Width { get; set; }

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

    [JsonPropertyName("content_type")] public string ContentType { get; set; }

    [JsonPropertyName("source")] public string Source { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }
}

public class Post
{
    [JsonPropertyName("attachment")] public Attachment Attachment { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }

    [JsonPropertyName("band")] public Band Band { get; set; }

    [JsonPropertyName("photo_count")] public int PhotoCount { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("web_url")] public string WebUrl { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }

    [JsonPropertyName("is_translatable")] public bool IsTranslatable { get; set; }

    [JsonPropertyName("video_count")] public int VideoCount { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("is_shareable")] public bool IsShareable { get; set; }

    [JsonPropertyName("shared_count")] public int SharedCount { get; set; }

    [JsonPropertyName("is_bookmarked")] public bool IsBookmarked { get; set; }

    [JsonPropertyName("read_count")] public int ReadCount { get; set; }

    [JsonPropertyName("should_disable_comment")]
    public bool ShouldDisableComment { get; set; }

    [JsonPropertyName("copiable_state")] public string CopiableState { get; set; }

    [JsonPropertyName("is_major_notice")] public bool IsMajorNotice { get; set; }

    [JsonPropertyName("common_emotion_type")]
    public List<string> CommonEmotionType { get; set; }

    [JsonPropertyName("written_in")] public string WrittenIn { get; set; }
}

public class Feed
{
    [JsonPropertyName("update_param")] public UpdateParam UpdateParam { get; set; }

    [JsonPropertyName("paging")] public Paging Paging { get; set; }

    [JsonPropertyName("items")] public List<Item> Items { get; set; }
}

public class Snippet
{
    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("image")] public string Image { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("domain")] public string Domain { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("seq")] public int Seq { get; set; }

    [JsonPropertyName("image_type")] public object ImageType { get; set; }

    [JsonPropertyName("image_width")] public int ImageWidth { get; set; }

    [JsonPropertyName("image_height")] public int ImageHeight { get; set; }

    [JsonPropertyName("video")] public object Video { get; set; }

    [JsonPropertyName("authorized_feature")]
    public object AuthorizedFeature { get; set; }

    [JsonPropertyName("oembed_html")] public object OembedHtml { get; set; }
}

public class UpdateParam
{
    [JsonPropertyName("updated_feeds_since")]
    public string UpdatedFeedsSince { get; set; }
}

public class Video
{
    [JsonPropertyName("has_chat")] public bool HasChat { get; set; }

    [JsonPropertyName("is_restricted")] public bool IsRestricted { get; set; }

    [JsonPropertyName("created_at")] public object CreatedAt { get; set; }

    [JsonPropertyName("height")] public int Height { get; set; }

    [JsonPropertyName("width")] public int Width { get; set; }

    [JsonPropertyName("post_no")] public int PostNo { get; set; }

    [JsonPropertyName("comment_count")] public int CommentCount { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("emotion_count")] public int EmotionCount { get; set; }

    [JsonPropertyName("photo_no")] public object PhotoNo { get; set; }

    [JsonPropertyName("author")] public Author Author { get; set; }

    [JsonPropertyName("logo_image")] public string LogoImage { get; set; }

    [JsonPropertyName("savable_state")] public string SavableState { get; set; }

    [JsonPropertyName("is_gif")] public bool IsGif { get; set; }

    [JsonPropertyName("is_soundless")] public bool IsSoundless { get; set; }

    [JsonPropertyName("is_live")] public bool IsLive { get; set; }
}