using Microsoft.AspNetCore.Identity;

namespace Website.Database;

public class Author : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}