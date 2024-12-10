using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Website.Areas.Admin.Pages.Tag
{
    public class IndexModel(Database.ApplicationDbContext context) : PageModel
    {
        public IList<Database.Tag> Tag { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Tag = await context.Tags.ToListAsync();
        }
    }
}