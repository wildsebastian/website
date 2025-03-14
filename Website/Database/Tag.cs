using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Website.Database;

[Index(nameof(Name), IsUnique = true)]
public class Tag
{
    [Key]
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string Name { get; init; } = null!;
    public List<BlogPostTag> BlogPostTags { get; } = [];
    public List<BlogPost> BlogPosts { get; } = [];
}