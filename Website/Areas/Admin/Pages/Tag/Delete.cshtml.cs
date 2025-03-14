using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website.Database;

namespace Website.Areas.Admin.Pages.Tag
{
    public class DeleteModel(ApplicationDbContext context) : PageModel
    {
        [BindProperty] public Database.Tag Tag { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await context.Tags.FirstOrDefaultAsync(m => m.Id == id);

            if (tag is null) return NotFound();
            Tag = tag;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await context.Tags.FindAsync(id);
            if (tag == null) return RedirectToPage("./Index");
            Tag = tag;
            context.Tags.Remove(Tag);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}