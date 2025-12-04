using WebProducto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// HttpClient para consumir WS
builder.Services.AddHttpClient();

// Registrar servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlmacenService, AlmacenService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
