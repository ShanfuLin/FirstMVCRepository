using ColorGridSSE.Models;

var builder = WebApplication.CreateBuilder(args);


var portVar = Environment.GetEnvironmentVariable("PORT");
if (portVar is { Length: > 0 } && int.TryParse(portVar, out int port))
{

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(port);
    });
}


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ColorChangeTracker>();

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

