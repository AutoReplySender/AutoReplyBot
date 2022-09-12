using Band;
using Band.Models.Comments;
using Band.Models.Feed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Item = Band.Models.Comments.Item;

namespace AutoReplyBot;

// Use Producer as name for historical reason, although it's not Producer-Consumer pattern anymore. 
public class Producer
{
    private readonly BandClient _bandClient;
    private readonly ILogger<Producer> _logger;
    private readonly Consumer _consumer;
    private readonly IServiceProvider _serviceProvider;

    public Producer(BandClient bandClient, ILogger<Producer> logger, Consumer consumer,
        IServiceProvider serviceProvider)
    {
        _bandClient = bandClient;
        _logger = logger;
        _consumer = consumer;
        _serviceProvider = serviceProvider;
    }

    public async Task Produce()
    {
        while (true)
        {
            _logger.LogInformation("Produce started");
            try
            {
                var feed = await _bandClient.GetFeedAsync();
                var tasks = feed.Items
                    .AsParallel()
                    .Select(i => i.Post)
                    .Select(ProcessPost)
                    .ToArray();
                // It's too fast so we must do something to slow it down
                await Task.WhenAll(tasks);
                _logger.LogInformation("Produce finished");
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                await _bandClient.RefreshAsync();
            }
        }
    }

    public async Task ProcessPost(Post? post)
    {
        // photo_album_compaction may appear in feed and shouldn't be checked.
        if (post == null) return;
        _logger.LogDebug("Processing Post {@Post}", post);
        var bandNo = post.Band.BandNo;
        var postNo = post.PostNo;
        await using var db = _serviceProvider.GetRequiredService<AutoReplyContext>();
        var it = new Comment(bandNo, postNo, 0, 0);
        var postTask = Task.CompletedTask;
        if (!await db.CheckProcessed(it))
            postTask = Task.Run(() =>
                _consumer.Consume(it, post.Content, post.Author.UserNo, post.Author.Name));
        var tasks = new[] { postTask };
        if (post.CommentCount != 0)
        {
            // Delay some time because it's too fast
            await Task.Delay(Random.Shared.Next(5000));
            var comments = await _bandClient.GetCommentsAsync(bandNo, postNo);
            tasks = comments.Items
                .AsParallel()
                .Select(item => ProcessComment(item, bandNo, postNo))
                .Append(postTask)
                .ToArray();
        }

        await Task.WhenAll(tasks);
    }

    public async Task ProcessComment(Item comment, int bandNo, int postNo)
    {
        await using var db = _serviceProvider.GetRequiredService<AutoReplyContext>();
        _logger.LogDebug("Processing Comment {@Comment}", comment);
        var commentId = comment.CommentId;
        var commentTask = Task.CompletedTask;
        var it = new Comment(bandNo, postNo, commentId, 0);
        if (!await db.CheckProcessed(it))
            commentTask = Task.Run(() => _consumer.Consume(it, comment.Body, comment.Author.UserNo,
                comment.Author.Name));

        var tasks = new[] { commentTask };
        if (comment.CommentCount != 0)
        {
            tasks = comment.LatestComment
                .AsParallel()
                .Select(subComment => ProcessSubComment(subComment, bandNo, postNo, commentId))
                .Append(commentTask)
                .ToArray();
        }

        await Task.WhenAll(tasks);
    }

    public async Task ProcessSubComment(LatestComment subComment, int bandNo, int postNo, int commentId)
    {
        await using var db = _serviceProvider.GetRequiredService<AutoReplyContext>();
        _logger.LogDebug("Processing SubComment {@SubComment}", subComment);
        var subCommentId = subComment.CommentId;
        var it = new Comment(bandNo, postNo, commentId, subCommentId);
        if (!await db.CheckProcessed(it))
            await _consumer.Consume(it, subComment.Body, subComment.Author.UserNo, subComment.Author.Name);
    }
}