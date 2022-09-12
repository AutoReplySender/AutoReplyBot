using System.Text.Json;
using Band.Models;
using Band.Models.Comments;
using Band.Models.Feed;

namespace Band;

public partial class BandClient
{
    public virtual Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, string body)
    {
        const string uri = "/v2.3.0/create_comment";
        var form = new CreateComment
        {
            BandNo = bandNo,
            Body = $"{body}\n\n{_suffix}",
            ContentKey = new ContentKey { PostNo = postNo }
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public virtual Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, int commentId, string body, int userNo,
        string userName)
    {
        const string uri = "/v2.3.0/create_comment";
        var form = new CreateComment
        {
            BandNo = bandNo,
            Body = $"<band:refer user_no=\"{userNo}\">{userName}</band:refer> {body}\n\n{_suffix}",
            ContentKey = new ContentKey { PostNo = postNo, ContentType = "post_comment", CommentId = commentId }
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public virtual Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, string type)
    {
        const string uri = "/v2.0.0/set_emotion";
        var form = new SetEmotion
        {
            BandNo = bandNo,
            Type = type,
            ContentKey = new ContentKey { PostNo = postNo }
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public virtual Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, int commentId, string type)
    {
        const string uri = "/v2.0.0/set_emotion";
        var form = new SetEmotion
        {
            BandNo = bandNo,
            Type = type,
            ContentKey = new ContentKey { PostNo = postNo, ContentType = "post_comment", CommentId = commentId }
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public virtual Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, int originalCommentId, int commentId,
        string type)
    {
        const string uri = "/v2.0.0/set_emotion";
        var form = new SetEmotion
        {
            BandNo = bandNo,
            Type = type,
            ContentKey = new ContentKey
            {
                PostNo = postNo, ContentType = "post_comment_comment", OriginalCommentId = originalCommentId,
                CommentId = commentId
            }
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public async Task<Comments> GetCommentsAsync(int bandNo, int postNo)
    {
        const string uri = "/v2.3.0/get_comments";
        var form = new
        {
            BandNo = bandNo,
            ContentKey = new ContentKey { ContentType = "post", PostNo = postNo }
        }.ToDictionary();
        using var response = await PostAsync(uri, form);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return (await JsonSerializer.DeserializeAsync<Result<Comments>>(stream))!.ResultData;
    }

    public async Task<Feed> GetFeedAsync()
    {
        const string uri = "/v2.1.0/get_feed";
        var form = new GetFeed().ToDictionary();
        using var response = await PostAsync(uri, form);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return (await JsonSerializer.DeserializeAsync<Result<Feed>>(stream))!.ResultData;
    }
}