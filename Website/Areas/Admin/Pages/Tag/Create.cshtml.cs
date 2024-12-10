using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Database;

namespace Website.Areas.Admin.Pages.Tag
{
    public class CreateModel(Website.Database.ApplicationDbContext context) : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Database.Tag Tag { get; set; } = null!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.Tags.Add(Tag);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}