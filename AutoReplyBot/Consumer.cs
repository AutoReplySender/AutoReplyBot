using Band;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoReplyBot;

// Use Consumer as name for historical reason, although it's not Producer-Consumer pattern anymore. 
public class Consumer
{
    private readonly BandClient _bandClient;
    private readonly Matcher _matcher;
    private readonly ILogger<Consumer> _logger;
    private readonly IServiceProvider _sp;

    public Consumer(BandClient bandClient, Matcher matcher, ILogger<Consumer> logger, IServiceProvider sp)
    {
        _bandClient = bandClient;
        _matcher = matcher;
        _logger = logger;
        _sp = sp;
    }

    public async Task Consume(Comment comment, string content, int userNo, string userName)
    {
        try
        {
            _logger.LogDebug("Start consuming {@Comment}", comment);
            await using var db = _sp.GetRequiredService<AutoReplyContext>();
            var actions = await _matcher.Match(comment, content, userNo, userName);
            // if no action is triggered, skip the comment so it may still trigger our bot after editing.
            if (actions.Length == 0) return;
            actions = actions.Where(r => (r.TriggerChance == null ||
                                          (r.TriggerChance != null &&
                                           r.TriggerChance > 100 * Random.Shared.NextDouble()))).ToArray();
            // if actions are triggered, but are randomly discarded, then it should never trigger the bot again.
            if (actions.Length == 0)
            {
                db.Add(comment);
                await db.SaveChangesAsync();
                return;
            }

            var (bandNo, postNo, commentId, subCommentId) = comment;
            var reply = string.Join("\n\n", actions.Select(a => a.ReplyContent));
            var emotion = actions.FirstOrDefault(a => a.EmotionType != null)?.EmotionType;
            _logger.LogInformation("Now replying {Reply} to {BandNo} {PostNo} {UserName} {Content}", reply,
                bandNo.ToString(), postNo.ToString(), userName, content);
            if (emotion != null)
            {
                _logger.LogInformation("Adding Emotion {Emotion} to {BandNo} {PostNo} {UserName} {Content}", emotion,
                    bandNo, postNo, userName, content);
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
            _logger.LogDebug("Finished consuming {@Comment}", comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
        }
    }
}