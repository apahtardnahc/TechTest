using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagement.Data;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Attempt fix at fixing persisting data between tests
//builder.Services.AddDbContext<DataContext>(options =>
//{
//    options.UseInMemoryDatabase("UserManagement.DefaultDatabase");
//});
builder.Services.AddDbContext<DataContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    options.UseNpgsql(connectionString);
});

builder.Logging.AddSimpleConsole(c => c.SingleLine = true);

builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

var app = builder.Build();

// Attempt fix at fixing persisting data between tests
//var scope = app.Services.CreateScope();
//var inMemoryDb = scope.ServiceProvider.GetRequiredService<DataContext>();
//inMemoryDb.Database.EnsureCreated();
//await using var scope = app.Services.CreateAsyncScope();
//var db = scope.ServiceProvider.GetRequiredService<DataContext>();
//db.Database.Migrate();
//var canConnect = await db.Database.CanConnectAsync();
//app.Logger.LogInformation("CAN CONNECT TO DATABASE: {CanConnect}", canConnect);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
