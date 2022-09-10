using System.Text.RegularExpressions;

namespace AutoReplyBot;

public class Matcher
{
    private readonly List<Rule> _rules;
    private readonly int _takes;

    public Matcher(List<Rule> rules, int takes)
    {
        _rules = rules;
        _takes = takes;
    }

    public class Action
    {
        public Action(string replyContent, string? emotionType, double? triggerChance)
        {
            ReplyContent = replyContent;
            EmotionType = emotionType;
            TriggerChance = triggerChance;
        }
        public string ReplyContent { get; set; }
        public string? EmotionType { get; set; }
        public double? TriggerChance { get; set; }
    }

    public Task<Action[]> Match(Comment? comment, string content, string userName)
    {
        // throw away @username when matching
        try
        {
            content = Regex.Replace(content, @"<band:refer[^>]*>[^<]*</band:refer>", "");
        }
        catch (RegexMatchTimeoutException e)
        {
            Console.WriteLine(e);
        }
        if (content.Contains("I am a bot")) return Task.FromResult(Array.Empty<Action>());
        var actions = _rules
            .AsParallel()
            .Where(r => (r.Keywords.Contains("*") ||
                        (r.IgnoreCase != false && r.Keywords.Any(k => content.Contains(k, StringComparison.OrdinalIgnoreCase))) ||
                        (r.IgnoreCase == false && r.Keywords.Any(content.Contains))) &&
                        (r.TargetAuthors.Contains(userName) || r.TargetAuthors.Contains("*")))
            .Where(r => (r.Type == null || (r.Type != null &&
                        ((r.Type == "post" && comment!.CommentId == 0) ||
                        (r.Type == "comment" && comment!.CommentId != 0)))))
            .Take(_takes)
            .Select(async r =>
            {
                var reply = r.Replies[Random.Shared.Next(r.Replies.Count)];
                return reply.ReplyType switch
                {
                    ReplyType.PlainText => new Action(reply.Data.Trim(), r.EmotionType, r.TriggerChance),
                    ReplyType.CSharpScript => new Action(await Script.Eval(reply.Data.Trim()), r.EmotionType, r.TriggerChance),
                    _ => throw new InvalidOperationException()
                };
            });
        return Task.WhenAll(actions);
    }
}