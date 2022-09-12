using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace AutoReplyBot;

public class AutoReplyContext : DbContext
{
    public DbSet<Comment> ProcessedComments { get; set; } = null!;

    public AutoReplyContext(DbContextOptions<AutoReplyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Comment>().HasKey(rc => new
        {
            rc.BandNo, rc.PostNo, rc.CommentId, rc.SubCommentId
        });
    }

    public async Task<bool> CheckProcessed(Comment comment)
    {
        return await ProcessedComments.AnyAsync(pc => pc == comment);
    }
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