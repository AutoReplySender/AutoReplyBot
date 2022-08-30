using System.Collections.Concurrent;
using Band;

namespace AutoReplyBot;

public class Producer
{
    private readonly BlockingCollection<ChannelData> _channel;
    private readonly BandClient _bandClient;

    public Producer(BlockingCollection<ChannelData> channel, BandClient bandClient)
    {
        _channel = channel;
        _bandClient = bandClient;
    }

    public async Task Produce()
    {
        await using var db = new AutoReplyContext();
        while (true)
        {
            Console.WriteLine("Produce started");
            try
            {
                var feed = await _bandClient.GetFeedAsync();
                foreach (var post in feed.Items.Select(i => i.Post))
                {
                    var bandNo = post.Band.BandNo;
                    var postNo = post.PostNo;
                    {
                        var it = new Comment(bandNo, postNo, 0, 0);
                        if (!await db.CheckProcessed(it))
                            _channel.Add(new ChannelData(it, post.Content, post.Author.UserNo, post.Author.Name));
                    }
                    if (post.CommentCount == 0) continue;
                    var comments = await _bandClient.GetCommentsAsync(bandNo, postNo);
                    foreach (var comment in comments.Items)
                    {
                        var commentId = comment.CommentId;
                        {
                            var it = new Comment(bandNo, postNo, commentId, 0);
                            if (!await db.CheckProcessed(it))
                                _channel.Add(new ChannelData(it, comment.Body, comment.Author.UserNo, comment.Author.Name));
                        }
                        if (comment.CommentCount == 0) continue;
                        foreach (var subComment in comment.LatestComment)
                        {
                            var subCommentId = subComment.CommentId;
                            var it = new Comment(bandNo, postNo, commentId, subCommentId);
                            if (!await db.CheckProcessed(it))
                                _channel.Add(new ChannelData(it, subComment.Body, subComment.Author.UserNo,
                                    subComment.Author.Name));
                        }
                    }
                }
                Console.WriteLine("Produce finished");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            await Task.Delay(5000);
        }
    }
}