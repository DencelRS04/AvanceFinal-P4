using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAlmacen;

namespace WebProducto.Pages.Proveedores
{
    public class CreateModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public CreateModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Proveedor Proveedor { get; set; } = new Proveedor();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Proveedor.Estado = "Activo";
            Proveedor.TipoTransaccion = "AGREGAR";

            var resultado = await _almacen.ProcesarProveedorAsync(Proveedor);

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
