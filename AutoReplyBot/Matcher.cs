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

    public Task<string[]> Match(string content, string userName)
    {
        if (content.Contains("I am a bot")) return Task.FromResult(Array.Empty<string>());
        var replies = _rules
            .AsParallel()
            .Where(r => (r.Keywords.Contains("*") ||
                        (r.IgnoreCase != false && r.Keywords.Any(k => content.Contains(k, StringComparison.OrdinalIgnoreCase))) ||
                        (r.IgnoreCase == false && r.Keywords.Any(content.Contains))) &&
                        (r.TargetAuthors.Contains(userName) || r.TargetAuthors.Contains("*")) &&
                        (r.TriggerChance == null ||
                        (r.TriggerChance != null && r.TriggerChance > Random.Shared.Next(100))))
            .Take(_takes)
            .Select(async r =>
            {
                var reply = r.Replies[Random.Shared.Next(r.Replies.Count)];
                return reply.ReplyType switch
                {
                    ReplyType.PlainText => reply.Data.Trim(),
                    ReplyType.CSharpScript => await Script.Eval(reply.Data.Trim()),
                    _ => throw new ArgumentOutOfRangeException()
                };
            });
        return Task.WhenAll(replies);
    }
}