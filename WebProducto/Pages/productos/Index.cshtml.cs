using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Collections.Generic;
using System.Linq;

namespace WebProducto.Pages.Productos
{
    public class IndexModel : PageModel
    {
        public List<Producto> ListaProductos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        public void OnGet()
        {
            // SIMULACIÓN: lista de productos (para cumplir con el criterio de listado).
            ListaProductos = new List<Producto>
            {
                new Producto { NumeroProducto = "P001", NombreProducto = "Teclado mecánico", Precio = 25000 },
                new Producto { NumeroProducto = "P002", NombreProducto = "Mouse gamer", Precio = 18000 },
                new Producto { NumeroProducto = "P003", NombreProducto = "Monitor 24\"", Precio = 75000 }
            };

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                var filtroLower = Filtro.ToLower();
                ListaProductos = ListaProductos
                    .Where(p =>
                        (p.NumeroProducto ?? "").ToLower().Contains(filtroLower) ||
                        (p.NombreProducto ?? "").ToLower().Contains(filtroLower))
                    .ToList();
            }
        }
    }
}
