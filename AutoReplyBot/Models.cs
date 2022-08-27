using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace AutoReplyBot;

public class AutoReplyContext : DbContext
{
    public DbSet<Rule> Rules { get; set; } = null!;
    public DbSet<Comment> ProcessedComments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options
            .UseNpgsql("Host=localhost;Database=postgres;Username=postgres;SearchPath=auto_reply;")
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Comment>().HasKey(rc => new
        {
            rc.BandNo, rc.PostNo, rc.CommentId, rc.SubCommentId
        });
    }
}

public enum ReplyType
{
    PlainText,
    [PgName("csharp_script")] CSharpScript
}

public class Rule
{
    public int Id { get; set; }
    public required List<string> Keywords { get; set; }
    public required List<string> TargetAuthors { get; set; }
    [Column(TypeName = "jsonb")] public required List<Reply> Replies { get; set; }
}

public class Reply
{
    public required ReplyType ReplyType { get; set; }
    public required string Data { get; set; }
}

[Table("processed_comments")]
public record Comment(int BandNo, int PostNo, int CommentId, int SubCommentId)
{
    public int BandNo { get; set; } = BandNo;
    public int PostNo { get; set; } = PostNo;

    // CommentId == 0 indicates that it's a post
    public int CommentId { get; set; } = CommentId;

    // SubCommentId == 0 indicates that it's a "parent" comment
    public int SubCommentId { get; set; } = SubCommentId;
}