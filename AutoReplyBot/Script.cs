using System.Collections.Concurrent;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace AutoReplyBot;

public class Script
{
    public static readonly ConcurrentDictionary<string, ScriptRunner<string>> FuncTable = new();

    public static Task<string> Eval(string code)
    {
        var compiled = FuncTable.TryGetValue(code, out var func);
        if (!compiled)
        {
            func = CSharpScript.Create<string>(code).CreateDelegate();
            FuncTable[code] = func;
        }

        return func!();
    }

    public static void Test()
    {
        const string code = $$"""
        $"网警已进入本群，网警{System.Random.Shared.Next(10_000, 100_000)}已经开始监控本群聊天"
        """ ;
        for (var i = 0; i < 100; i++)
        {
            Console.WriteLine(Eval(code).Result);
        }
    }
}