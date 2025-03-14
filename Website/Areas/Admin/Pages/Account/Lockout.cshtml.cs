// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Areas.Admin.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
    }
}