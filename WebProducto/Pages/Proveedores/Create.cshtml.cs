using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Threading.Tasks;
using WebProducto.Services;

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
        public Proveedor Prov { get; set; } = new Proveedor();

        public void OnGet()
        {
            Prov.Estado = "Activo";
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

            Prov.TipoTransaccion = "1"; // insertar

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
