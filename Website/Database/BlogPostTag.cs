using Microsoft.EntityFrameworkCore;

namespace Website.Database;

[PrimaryKey(nameof(BlogPostId), nameof(TagId))]
public class BlogPostTag
{
    public Guid BlogPostId { get; set; }
    public Guid TagId { get; set; }
    public BlogPost BlogPost { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}