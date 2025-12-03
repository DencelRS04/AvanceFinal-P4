using Microsoft.EntityFrameworkCore;
using BibliotecaORM.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//  **************   Ambiente BD
builder.Services.AddDbContext<Ambiente>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:DatabaseConnection");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    Ambiente context = scope.ServiceProvider.GetRequiredService<Ambiente>();
    context.Database.EnsureCreated();
}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
