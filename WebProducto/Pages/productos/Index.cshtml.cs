using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAlmacen;

namespace WebProducto.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public IndexModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        public List<Producto> Productos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Productos = await _almacen.ListarProductosAsync();
        }
    }
}
