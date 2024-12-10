using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Website.Database;

namespace Website.Areas.Admin.Pages.Blog
{
    public class Create(ApplicationDbContext context) : PageModel
    {
        public IList<Author> Authors { get; set; } = null!;

        [BindProperty] public BlogPost? BlogPost { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Authors = await context.Authors.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (BlogPost != null)
            {
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().DisableHtml().Build();
                if (BlogPost.Content != null)
                {
                    var contentHtml = Markdown.ToHtml(BlogPost.Content, pipeline);
                    BlogPost.ContentHtml = contentHtml;
                }

                BlogPost.CreatedAt = Instant.FromDateTimeUtc(DateTime.UtcNow);
                context.BlogPosts.Add(BlogPost);
            }

            await context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}