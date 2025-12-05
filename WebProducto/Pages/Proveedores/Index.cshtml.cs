using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Collections.Generic;
using System.Linq;

namespace WebProducto.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        public List<Proveedor> ListaProveedores { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        public void OnGet()
        {
            // SIMULACIÓN de proveedores
            ListaProveedores = new List<Proveedor>
            {
                new Proveedor { CedulaJuridica = "3010101010", NombreEmpresa = "Tech Solutions", NombreContacto="Carlos Pérez", Telefono="8888-8888", Correo="info@tech.com", Estado="Activo" },
                new Proveedor { CedulaJuridica = "3020202020", NombreEmpresa = "Distribuidora Nova", NombreContacto="Ana López", Telefono="8999-9999", Correo="contacto@nova.com", Estado="Inactivo" }
            };

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                var f = Filtro.ToLower();
                ListaProveedores = ListaProveedores
                    .Where(p =>
                        (p.CedulaJuridica ?? "").ToLower().Contains(f) ||
                        (p.NombreEmpresa ?? "").ToLower().Contains(f) ||
                        (p.NombreContacto ?? "").ToLower().Contains(f) ||
                        (p.Telefono ?? "").ToLower().Contains(f) ||
                        (p.Correo ?? "").ToLower().Contains(f) ||
                        (p.Estado ?? "").ToLower().Contains(f))
                    .ToList();
            }
        }
    }
}
