using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website.Database;

namespace Website.Areas.Admin.Pages.Tag
{
    public class DetailsModel(ApplicationDbContext context) : PageModel
    {
        public Database.Tag Tag { get; set; } = null!;

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
    }
}