using Microsoft.EntityFrameworkCore;

namespace Website.Database;

[PrimaryKey(nameof(BlogPostId), nameof(TagId))]
public class BlogPostTag
{
    public Guid BlogPostId { get; init; }
    public Guid TagId { get; init; }
    public BlogPost BlogPost { get; init; } = null!;
    public Tag Tag { get; init; } = null!;
}