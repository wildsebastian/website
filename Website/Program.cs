using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.ModelBinder;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException(
                         "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, o => o.UseNodaTime());
    options.UseOpenIddict();
});

builder.Services.AddIdentity<Author, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

var gitHubClientId = builder.Configuration.GetValue<string>("GitHub:ClientId", "");
var githubClientSecret = builder.Configuration.GetValue<string>("GitHub:ClientSecret", "");

builder.Services.AddOpenIddict()
  .AddCore(options =>
  {
      options.UseEntityFrameworkCore()
        .UseDbContext<ApplicationDbContext>();
  })
  .AddClient(options =>
  {
      options.AllowAuthorizationCodeFlow();
      options.AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate();
      options.UseAspNetCore()
        .EnableRedirectionEndpointPassthrough();
      options.UseSystemNetHttp();
      options.UseWebProviders()
        .AddGitHub(options =>
        {
            options.SetClientId(gitHubClientId)
            .SetClientSecret(githubClientSecret)
            .SetRedirectUri("callback/login/github");
        });
  });

builder.Services.ConfigureApplicationCookie(options =>
  {
      // Cookie settings
      options.Cookie.HttpOnly = true;
      options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

      options.LoginPath = new PathString("/admin/account/login");
      options.AccessDeniedPath = new PathString("/admin/account/accessdenied");
      options.SlidingExpiration = true;
  }
);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions
    .AuthorizeAreaFolder("Admin", "/")
    .AllowAnonymousToAreaPage("Admin", "/Account/Login")
    .AllowAnonymousToAreaPage("Admin", "/Account/ExternalLogin")
    .AllowAnonymousToAreaPage("Admin", "/Account/AccessDenied");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.MapStaticAssets();
app.MapRazorPages()
  .WithStaticAssets();

app.Run();