using ServiceReferenceAlmacen;
using ServiceReferenceAutenticacion;
using WebProducto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// ?? AGREGAR SESSION ANTES DE builder.Build()
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// WCF Clients
builder.Services.AddTransient<ServiceClient>();
builder.Services.AddTransient<AutenticacionClient>();

// Services
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<IAlmacenService, AlmacenService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ?? AGREGAR UseSession() ANTES de MapRazorPages
app.UseSession();

app.MapGet("/", () => Results.Redirect("/Account/Login"));

app.MapRazorPages();
app.Run();