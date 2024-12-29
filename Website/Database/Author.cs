using Microsoft.AspNetCore.Identity;

namespace Website.Data;

public class Author : IdentityUser
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}
