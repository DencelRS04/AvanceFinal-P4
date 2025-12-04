using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAlmacen;

namespace WebProducto.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public CreateModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // TipoTransaccion se usa en el WS para saber qué hacer
            Producto.TipoTransaccion = "AGREGAR";

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
