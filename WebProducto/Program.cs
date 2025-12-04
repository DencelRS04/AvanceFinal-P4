using WebProducto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Inyección de dependencias
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlmacenService, AlmacenService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
