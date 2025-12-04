using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;

namespace WebProducto.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public IndexModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        public List<ProductoDTO> Productos { get; set; }

        public async Task OnGetAsync()
        {
            Productos = await _almacen.ObtenerProductos();
        }
    }
}
