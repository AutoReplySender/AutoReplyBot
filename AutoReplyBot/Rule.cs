using Microsoft.CodeAnalysis.Scripting;

namespace AutoReplyBot;

public class Rule
{
    public int Id { get; set; }
    public required List<string> Keywords { get; set; }
    public required List<string> TargetAuthors { get; set; }
    public required List<Reply> Replies { get; set; }
    public bool? IgnoreCase { get; set; }
    public double? TriggerChance { get; set; }
    public string? Type { get; set; }
    public bool? AtMe { get; set; }
}

public class Reply
{
    public required ReplyType ReplyType { get; set; }

    public required string Data { get; set; }

    // Will be null when ReplyType is PlainText. Maybe there is something better if C# has discriminated union.
    public ScriptRunner<string>? Script;

    public string? EmotionType { get; set; }
}

public enum ReplyType
{
    PlainText,
    CSharpScript
}