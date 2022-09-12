using System.Collections.Concurrent;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace AutoReplyBot;

public class Script
{
    public static ScriptRunner<TResult> CreateDelegate<TResult>(string code, Type? globalsType = null)
    {
        return CSharpScript.Create<TResult>(code, globalsType: globalsType).CreateDelegate();
    }
}