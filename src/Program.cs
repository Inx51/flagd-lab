using Flagd.Options;
using Microsoft.Extensions.Options;
using OpenFeature;
using OpenFeature.Contrib.Providers.Flagd;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<FlagdOptions>(builder.Configuration.GetSection(FlagdOptions.SectionName));

var options = new FlagdOptions();
builder.Configuration.GetSection(FlagdOptions.SectionName).Bind(options);
await Api.Instance.SetProviderAsync
(
    new FlagdProvider
    (
        new Uri(options.Endpoint!)
    )
);
builder.Services.AddSingleton(Api.Instance.GetClient());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
