using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website.Database;

namespace Website.Areas.Admin.Pages.Blog;

public class Edit(ApplicationDbContext context) : PageModel
{
    public IList<Author> Authors { get; set; } = null!;

    [BindProperty] public BlogPost BlogPost { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Authors = await context.Authors.ToListAsync();

        var blogPost = await context.BlogPosts.Include(post => post.Author).FirstOrDefaultAsync(m => m.Id == id);
        if (blogPost == null)
        {
            return NotFound();
        }

        BlogPost = blogPost;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Attach(BlogPost).State = EntityState.Modified;

        try
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().DisableHtml().Build();
            if (BlogPost.Content != null)
            {
                var contentHtml = Markdown.ToHtml(BlogPost.Content, pipeline);
                BlogPost.ContentHtml = contentHtml;
            }

            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BlogPostExists(BlogPost.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool BlogPostExists(Guid id)
    {
        return context.BlogPosts.Any(e => e.Id == id);
    }
}