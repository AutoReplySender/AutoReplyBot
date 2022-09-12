using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Band;

public static class Helper
{
    public static List<Cookie> ParseCookies(string cookies)
    {
        var list = new List<Cookie>();
        var entries = cookies.Trim().Split("; ");
        foreach (var entry in entries)
        {
            var index = entry.IndexOf('=');
            var key = entry[..index];
            var value = entry[(index + 1)..];
            list.Add(new Cookie(key, value));
        }

        return list;
    }

    public static long GetUnixTimeStamp()
    {
        // UtcNow is slightly faster
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public static IDictionary<string, string> ToDictionary(this object source)
    {
        return source
            .GetType()
            .GetProperties()
            .ToDictionary(
                property => property.Name.ToSnakeCase(),
                property => property.GetValue(source)?.ToString() ?? ""
            );
    }

    public static string ToSnakeCase(this string s)
    {
        if (s.Length == 0) return "";
        var sb = new StringBuilder();
        sb.Append(char.ToLower(s[0]));
        foreach (var c in s[1..])
            if (char.IsLower(c))
            {
                sb.Append(c);
            }
            else
            {
                sb.Append('_');
                sb.Append(char.ToLower(c));
            }

        return sb.ToString();
    }

    public static string PrettyFormat(this object source)
    {
        return JsonSerializer.Serialize(source,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
    }
}