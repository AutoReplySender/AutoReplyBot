using Xunit;
using Xunit.Abstractions;

namespace AutoReplyBot.Tests;

public class UnitTestScript
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTestScript(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestScript()
    {
        var func = Script.CreateDelegate<int>("1 + 1");
        Assert.Equal(2, await func());
    }

    [Fact]
    public async Task TestMoreScript()
    {
        const string code = $$"""
        $"网警已进入本群，网警{System.Random.Shared.Next(10_000, 100_000)}已经开始监控本群聊天"
        """ ;
        var func = Script.CreateDelegate<string>(code);
        for (var i = 0; i < 10; i++)
        {
            _testOutputHelper.WriteLine(await func());
        }
    }

    [Fact]
    public async Task TestLongScript()
    {
        const string code = $$"""
        var a = 1;
        var b = 2;
        $"a + b = {a + b}"
        """ ;
        var func = Script.CreateDelegate<string>(code);
        Assert.Equal("a + b = 3", await func());
    }

    public record TestData
    {
        public int RandomNumber = Random.Shared.Next(100);
    }

    [Fact]
    public async Task TestScriptWithGlobalVariables()
    {
        const string code = $$"""
        $"Got RandomNumber from global variables: {RandomNumber}"
        """ ;
        var func = Script.CreateDelegate<string>(code, typeof(TestData));
        for (var i = 0; i < 10; i++)
        {
            _testOutputHelper.WriteLine(await func(new TestData()));
        }
    }
}