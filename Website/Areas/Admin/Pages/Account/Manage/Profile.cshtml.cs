// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Website.Database;

namespace Website.Areas.Admin.Pages.Account.Manage
{
    public class IndexModel(
        UserManager<Author> userManager,
        SignInManager<Author> signInManager)
        : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public InputModel()
            {
            }

            public InputModel(string firstName, string lastName, string phoneNumber)
            {
                FirstName = firstName;
                LastName = lastName;
                PhoneNumber = phoneNumber;
            }

            [Display(Name = "First name")] public string FirstName { get; init; }

            [Display(Name = "Last name")] public string LastName { get; init; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; init; }
        }

        private async Task LoadAsync(Author user)
        {
            string userName = await userManager.GetUserNameAsync(user);
            string phoneNumber = await userManager.GetPhoneNumberAsync(user);
            Author dbUser = await userManager.FindByNameAsync(userName ?? string.Empty);

            Username = userName;

            Input = new InputModel(firstName: dbUser?.FirstName, lastName: dbUser?.LastName, phoneNumber: phoneNumber);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }


            // Update FirstName and LastName attributes if they have changed
            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update the first name.";
                    return RedirectToPage();
                }
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update the last name.";
                    return RedirectToPage();
                }
            }

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}