// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Database;

namespace Website.Areas.Admin.Pages.Account
{
    public class LogoutModel(SignInManager<Author> signInManager)
        : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            await signInManager.SignOutAsync();
            return LocalRedirect(Url.Content("/Admin/Account/Login"));
        }
    }
}