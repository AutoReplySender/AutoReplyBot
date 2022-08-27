using System.Collections.Concurrent;
using Band;

namespace AutoReplyBot;

public class Consumer
{
    private readonly BlockingCollection<ChannelData> _channel;
    private readonly BandClient _bandClient;
    private readonly Matcher _matcher;

    public Consumer(BlockingCollection<ChannelData> channel, BandClient bandClient, Matcher matcher)
    {
        _channel = channel;
        _bandClient = bandClient;
        _matcher = matcher;
    }

    public async Task Consume()
    {
        var db = new AutoReplyContext();
        while (true)
        {
            var (comment, content, userNo, userName) = _channel.Take();
            var reply = await _matcher.Match(content, userName);
            if (reply == null) continue;
            if (!await db.CheckProcessed(comment))
            {
                var (bandNo, postNo, commentId, _) = comment;
                Console.WriteLine($"Now replying {reply} to {bandNo} {postNo} {userName} {content}");
                switch (comment)
                {
                    case (_, _, 0, 0):
                        await _bandClient.CreateCommentAsync(bandNo, postNo, reply);
                        break;
                    default:
                        await _bandClient.CreateCommentAsync(bandNo, postNo, commentId, reply, userNo, userName);
                        break;
                }
            }

            db.Add(comment);
            await db.SaveChangesAsync();
        }
    }
}