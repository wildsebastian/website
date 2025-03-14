using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Website.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
    );
    options.UseNpgsql(connectionString, o => o.UseNodaTime());
    options.UseOpenIddict();
});

builder.Services.AddIdentity<Author, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

string gitHubClientId = builder.Configuration.GetValue<string>("GitHub:ClientId", "");
string githubClientSecret = builder.Configuration.GetValue<string>("GitHub:ClientSecret", "");

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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();
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

await app.RunAsync();