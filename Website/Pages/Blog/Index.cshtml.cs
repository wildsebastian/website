using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Website.Database;

namespace Website.Pages.Blog;

public class Index(ApplicationDbContext context) : PageModel
{
    public IList<BlogPost> BlogPosts { get; set; } = null!;
    public BlogPost? BlogPost { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(string? slug)
    {
        if (slug == null)
        {
            var now = Instant.FromDateTimeUtc(DateTime.UtcNow);
            BlogPosts = await context.BlogPosts
                  .Include(post => post.Author)
                  .Where(post => post.PublishedAt <= now)
                  .OrderByDescending(post => post.PublishedAt)
                  .ToListAsync();

            return Page();
        }

        BlogPost = await context.BlogPosts.Include(post => post.Author).FirstOrDefaultAsync(post => post.Slug == slug);

        return Page();
    }
}