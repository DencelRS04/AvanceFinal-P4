using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using WebProducto.Services;
using System.Threading.Tasks;

namespace WebProducto.Pages.Proveedores
{
    public class EditModel : PageModel
    {
        private readonly IAlmacenService _almacenService;

        public EditModel(IAlmacenService almacenService)
        {
            _almacenService = almacenService;
        }

        [BindProperty]
        public Proveedor Proveedor { get; set; } = new Proveedor();

        public void OnGet(string cedula, string empresa, string contacto, string telefono, string correo, string estado)
        {
            Proveedor.CedulaJuridica = cedula;
            Proveedor.NombreEmpresa = empresa;
            Proveedor.NombreContacto = contacto;
            Proveedor.Telefono = telefono;
            Proveedor.Correo = correo;
            Proveedor.Estado = estado;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Proveedor.TipoTransaccion = "M"; // Modificar

            var resultado = await _almacenService.ProcesarProveedorAsync(Proveedor);

            if (resultado != null && resultado.ResultadoOperacion)
            {
                TempData["Mensaje"] = "¡Modificación finalizada de forma exitosa!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Error al realizar el proceso: " + resultado?.Mensaje);
            return Page();
        }
    }
}
