using System.Net;
using Band;
using Microsoft.Extensions.Logging;

namespace AutoReplyBot;

public class MockBandClient : BandClient
{
    public MockBandClient(BandClientOptions options, HttpPing httpPing, ILogger<MockBandClient> logger,
        IWebProxy? proxy = null) : base(options, httpPing, logger, proxy)
    {
    }

    public override Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, string body)
    {
        Logger.LogDebug("Mock CreateCommentAsync");
        return Task.FromResult(new HttpResponseMessage());
    }

    public override Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, int commentId, string body,
        int userNo, string userName)
    {
        Logger.LogDebug("Mock CreateCommentAsync");
        return Task.FromResult(new HttpResponseMessage());
    }

    public override Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, string type)
    {
        Logger.LogDebug("Mock SetEmotionAsync");
        return Task.FromResult(new HttpResponseMessage());
    }

    public override Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, int commentId, string type)
    {
        Logger.LogDebug("Mock SetEmotionAsync");
        return Task.FromResult(new HttpResponseMessage());
    }

    public override Task<HttpResponseMessage> SetEmotionAsync(int bandNo, int postNo, int originalCommentId,
        int commentId, string type)
    {
        Logger.LogDebug("Mock SetEmotionAsync");
        return Task.FromResult(new HttpResponseMessage());
    }
}