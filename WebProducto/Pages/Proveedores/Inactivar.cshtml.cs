using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Threading.Tasks;
using WebProducto.Services;

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
        public Proveedor Prov { get; set; } = new Proveedor();

        public IActionResult OnGet(string cedula)
        {
            Prov.CedulaJuridica = cedula;
            Prov.Estado = "Inactivo";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Prov.TipoTransaccion = "4"; // actualizar / cambio de estado

            var resp = await _almacen.ProcesarProveedorAsync(Prov);

            if (!resp.ResultadoOperacion)
            {
                ModelState.AddModelError("", "Error al realizar el proceso: " + resp.Mensaje);
                return Page();
            }

            TempData["MensajeExitoProveedores"] = "¡Proceso finalizado de forma exitosa!";

            return RedirectToPage("Index");
        }
    }
}
