namespace AutoReplyBot;

public class Matcher
{
    private readonly List<Rule> _rules;

    public Matcher(List<Rule> rules)
    {
        _rules = rules;
    }

    public async Task<string?> Match(string content)
    {
        if (content.Contains("I am a bot")) return null;
        var rule = _rules.FirstOrDefault(r => r.Keywords.Any(content.Contains));
        if (rule == null) return null;
        var reply = rule.Replies[Random.Shared.Next(rule.Replies.Count)];
        return reply.ReplyType switch
        {
            ReplyType.PlainText => reply.Data,
            ReplyType.CSharpScript => await Script.Eval(reply.Data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}