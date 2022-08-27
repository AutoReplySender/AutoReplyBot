using Microsoft.EntityFrameworkCore;

namespace AutoReplyBot;

public static class Common
{
    public static async Task<bool> CheckProcessed(this AutoReplyContext context, Comment comment)
    {
        return await context.ProcessedComments.AnyAsync(pc => pc == comment);
    }

    public static void Test()
    {
        var result = CheckProcessed(new AutoReplyContext(), new Comment(1, 2, 3, 4)).Result;
        Console.WriteLine(result);
    }
}

public record ChannelData(Comment Comment, string Content, int UserNo, string UserName);