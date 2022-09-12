using Band;
using Xunit;
using Xunit.Abstractions;

namespace AutoReplyBot.Tests;

public class UnitTestRule
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTestRule(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestRule()
    {
        var rules = await Program.LoadRules();
        _testOutputHelper.WriteLine(rules.PrettyFormat());
    }

    [Fact]
    public async Task TestColdbirdbird()
    {
        var rules = await Program.LoadRules();
        var theRule = rules.First(rule => rule.Replies.Any(reply => reply.Data.Contains("我一定要")));
        var theReplies = theRule.Replies.Where(reply => reply.ReplyType == ReplyType.CSharpScript);
        var global = new Global("Coldbirdbird", 110285631);
        foreach (var reply in theReplies)
        {
            _testOutputHelper.WriteLine(await reply.Script!(global));
        }
    }
}