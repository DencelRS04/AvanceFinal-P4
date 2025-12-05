using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceReferenceAlmacen;
using ServiceReferenceAuth;
using WebProducto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Sesión para guardar el usuario logueado
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Clientes WCF (Service References)
builder.Services.AddTransient<ServiceClient>();         // WS_ALMACEN
builder.Services.AddTransient<AutenticacionClient>();   // WS_AUTENTICACION


// Servicios de dominio
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

app.UseSession();   // MUY IMPORTANTE: antes de MapRazorPages

app.MapRazorPages();

app.Run();
