using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Website.Database;

namespace Website.Pages;

public class IndexModel(ApplicationDbContext context) : PageModel
{
    public IList<BlogPost> BlogPosts { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var now = Instant.FromDateTimeUtc(DateTime.UtcNow);
        BlogPosts = await context.BlogPosts
          .Include(post => post.Author)
          .Where(post => post.PublishedAt <= now)
          .OrderByDescending(post => post.PublishedAt)
          .Take(5)
          .ToListAsync();
        return Page();
    }
}