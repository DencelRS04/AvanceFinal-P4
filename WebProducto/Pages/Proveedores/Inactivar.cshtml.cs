using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAlmacen;

namespace WebProducto.Pages.Proveedores
{
    public class InactivarModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public InactivarModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Proveedor Proveedor { get; set; } = new Proveedor();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var lista = await _almacen.ListarProveedoresAsync();
            Proveedor = lista.FirstOrDefault(p => p.CedulaJuridica == id);

            if (Proveedor == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resultado = await _almacen.InactivarProveedorAsync(Proveedor.CedulaJuridica);

            if (resultado != null && resultado.ResultadoOperacion)
            {
                TempData["Mensaje"] = "¡Modificación finalizada de forma exitosa!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty,
                "Error al realizar el proceso: " + (resultado?.Mensaje ?? "Error desconocido"));

            return Page();
        }
    }
}
