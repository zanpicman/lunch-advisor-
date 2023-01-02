using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LunchAdvisor.Data;
using LunchAdvisor.Areas.Identity.Data;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AzureContext") ?? throw new InvalidOperationException("Connection string 'AzureContext' not found.");

builder.Services.AddDbContext<LunchAdvisorContext>(options =>
    options.UseSqlServer(connectionString));
/*
builder.Services.AddDefaultIdentity<LunchAdvisorUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LunchAdvisorContext>();
*/

builder.Services.AddDefaultIdentity<LunchAdvisorUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<LunchAdvisorContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(),@"Assets")),
            RequestPath =  new PathString("/assets")
});


app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
