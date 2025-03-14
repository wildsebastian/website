using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website.Database;

namespace Website.Areas.Admin.Pages.Blog
{
    public class IndexModel(ApplicationDbContext context) : PageModel
    {
        public IList<BlogPost> BlogPosts { get; set; } = null!;

        public async Task OnGetAsync()
        {
            BlogPosts = await context.BlogPosts.Select(post =>
              new BlogPost
              {
                  Id = post.Id,
                  Title = post.Title,
                  Abstract = post.Abstract,
                  CreatedAt = post.CreatedAt,
                  UpdatedAt = post.UpdatedAt,
                  PublishedAt = post.PublishedAt
              }).OrderBy(post => post.CreatedAt).ToListAsync();
        }
    }
}