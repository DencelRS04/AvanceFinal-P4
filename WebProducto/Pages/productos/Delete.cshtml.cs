using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblioteca.Models;
using WebProducto.Services;

namespace WebProducto.Pages.Productos
{
    public class DeleteModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        [BindProperty]
        public ProductoDTO Producto { get; set; }

        public DeleteModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _almacen.ObtenerProducto(id);

            if (Producto == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Esto depende si tu WS permite eliminar
            var resp = await _almacen.ModificarProducto(
                new ProductoDTO
                {
                    NumeroProducto = Producto.NumeroProducto,
                    Nombre = Producto.Nombre,
                    Precio = Producto.Precio
                }
            );

            return RedirectToPage("./Index");
        }
    }
}
