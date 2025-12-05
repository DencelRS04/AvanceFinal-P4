using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Threading.Tasks;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class EditModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public EditModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Proveedor Prov { get; set; } = new Proveedor();

        public IActionResult OnGet(string cedula)
        {
            // Aquí deberías cargar el proveedor real.
            // Simulación: rellenar solo la cédula.
            Prov.CedulaJuridica = cedula;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Prov.CedulaJuridica) ||
                string.IsNullOrWhiteSpace(Prov.NombreEmpresa) ||
                string.IsNullOrWhiteSpace(Prov.NombreContacto) ||
                string.IsNullOrWhiteSpace(Prov.Telefono) ||
                string.IsNullOrWhiteSpace(Prov.Correo))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios.");
                return Page();
            }

            Prov.TipoTransaccion = "4"; // actualizar

            var resp = await _almacen.ProcesarProveedorAsync(Prov);

            if (!resp.ResultadoOperacion)
            {
                ModelState.AddModelError("", "Error al realizar el proceso: " + resp.Mensaje);
                return Page();
            }

            TempData["MensajeExitoProveedores"] = "¡Modificación finalizada de forma exitosa!";

            return RedirectToPage("Index");
        }
    }
}
