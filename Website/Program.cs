using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException(
                         "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseNpgsql(connectionString);
  options.UseOpenIddict();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
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

    options.LoginPath = new PathString("/Admin/Account/Login");
    options.AccessDeniedPath = new PathString("/Admin/Account/AccessDenied");
    options.SlidingExpiration = true;
  }
);

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseEndpoints(options =>
  {
    options.MapControllers();
    options.MapDefaultControllerRoute();
  }
);

app.MapStaticAssets();
app.MapRazorPages()
  .WithStaticAssets();

app.Run();
