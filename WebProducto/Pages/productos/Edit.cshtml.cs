using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAlmacen;

namespace WebProducto.Pages.Productos
{
    public class EditModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public EditModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var lista = await _almacen.ListarProductosAsync();
            Producto = lista.FirstOrDefault(p => p.NumeroProducto == id);

            if (Producto == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Producto.TipoTransaccion = "MODIFICAR";

            var resultado = await _almacen.ProcesarProductoAsync(Producto);

            if (resultado != null && resultado.ResultadoOperacion)
            {
                TempData["Mensaje"] = "¡Proceso finalizado de forma exitosa!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty,
                "Error al realizar el proceso: " + (resultado?.Mensaje ?? "Error desconocido"));

            return Page();
        }
    }
}
