using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public IndexModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        public List<ProveedorDTO> Proveedores { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        public async Task OnGetAsync()
        {
            var lista = await _almacen.ObtenerProveedores();

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                lista = lista.Where(p =>
                    (p.CedulaJuridica ?? "").Contains(Filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Empresa ?? "").Contains(Filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Contacto ?? "").Contains(Filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Telefono ?? "").Contains(Filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Estado ?? "").Contains(Filtro, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            Proveedores = lista;
        }
    }
}
