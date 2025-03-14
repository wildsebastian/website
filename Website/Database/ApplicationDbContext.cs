using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Website.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<Author>(options)
{
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<BlogPost> BlogPosts { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<BlogPostTag> BlogPostTags { get; set; } = null!;
}