using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Threading.Tasks;
using WebProducto.Services;

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
        public Producto Prod { get; set; } = new Producto();

        public IActionResult OnGet(string numero)
        {
            // Aquí deberías cargar el producto desde BD/WS.
            // Se simula llenando solo el código:
            Prod.NumeroProducto = numero;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Prod.NumeroProducto) ||
                string.IsNullOrWhiteSpace(Prod.NombreProducto))
            {
                ModelState.AddModelError("", "Debe indicar el código y nombre del producto.");
                return Page();
            }

            if (Prod.Precio <= 0)
            {
                ModelState.AddModelError("", "El precio debe ser mayor a cero.");
                return Page();
            }

            Prod.TipoTransaccion = "4"; // actualizar

            var resp = await _almacen.ProcesarProductoAsync(Prod);

            if (!resp.ResultadoOperacion)
            {
                ModelState.AddModelError("", "Error al realizar el proceso: " + resp.Mensaje);
                return Page();
            }

            TempData["MensajeExitoProductos"] = "¡Proceso finalizado de forma exitosa!";

            return RedirectToPage("Index");
        }
    }
}
