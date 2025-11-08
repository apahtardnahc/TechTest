using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Attempt fix at fixing persisting data between tests
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseInMemoryDatabase("UserManagement.DefaultDatabase");
});



builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

var app = builder.Build();

// Attempt fix at fixing persisting data between tests
var scope = app.Services.CreateScope();
var inMemoryDb = scope.ServiceProvider.GetRequiredService<DataContext>();
inMemoryDb.Database.EnsureCreated();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
