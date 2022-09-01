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
            try
            {
                var (comment, content, userNo, userName) = _channel.Take();
                if (await db.CheckProcessed(comment)) continue;
                var actions = await _matcher.Match(content, userName);
                if (actions.Length == 0) continue;
                var (bandNo, postNo, commentId, subCommentId) = comment;
                var reply = string.Join("\n\n", actions.Select(a => a.ReplyContent));
                var emotion = actions.FirstOrDefault(a => a.EmotionType != null)?.EmotionType;
                Console.WriteLine($"Now replying {reply} to {bandNo} {postNo} {userName} {content}");
                if (emotion != null)
                {
                    Console.WriteLine(
                        $"Adding Emotion {emotion} to {bandNo} {postNo} {userName} {content}");
                }

                switch (comment)
                {
                    case (_, _, 0, 0):
                        await _bandClient.CreateCommentAsync(bandNo, postNo, reply);
                        if (emotion != null)
                        {
                            await _bandClient.SetEmotionAsync(bandNo, postNo, reply);
                        }

                        break;
                    case (_, _, _, 0):
                        await _bandClient.CreateCommentAsync(bandNo, postNo, commentId, reply, userNo,
                            userName);
                        if (emotion != null)
                        {
                            await _bandClient.SetEmotionAsync(bandNo, postNo, commentId, emotion);
                        }

                        break;
                    default:
                        await _bandClient.CreateCommentAsync(bandNo, postNo, commentId, reply, userNo,
                            userName);
                        if (emotion != null)
                        {
                            await _bandClient.SetEmotionAsync(bandNo, postNo, commentId, subCommentId,
                                emotion);
                        }

                        break;
                }

                db.Add(comment);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _bandClient.RefreshAsync().Wait();
            }
        }
    }
}