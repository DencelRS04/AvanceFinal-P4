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
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IAlmacenService almacen, ILogger<CreateModel> logger)
        {
            _almacen = almacen;
            _logger = logger;
        }

        [BindProperty]
        public Proveedor Prov { get; set; } = new Proveedor();

        public void OnGet()
        {
            _logger.LogInformation("Página Create/Proveedores cargada");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO CREAR PROVEEDOR ===");

                // Validaciones
                if (string.IsNullOrWhiteSpace(Prov.CedulaJuridica))
                {
                    ModelState.AddModelError("Prov.CedulaJuridica", "La cédula jurídica es requerida.");
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Prov.NombreEmpresa))
                {
                    ModelState.AddModelError("Prov.NombreEmpresa", "El nombre de la empresa es requerido.");
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Prov.NombreContacto))
                {
                    ModelState.AddModelError("Prov.NombreContacto", "El nombre del contacto es requerido.");
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Prov.Telefono))
                {
                    ModelState.AddModelError("Prov.Telefono", "El teléfono es requerido.");
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Prov.Correo))
                {
                    ModelState.AddModelError("Prov.Correo", "El correo electrónico es requerido.");
                    return Page();
                }

                // CORRECCIÓN CRÍTICA: El TipoTransaccion debe ser "3" para crear proveedor
                Prov.TipoTransaccion = "3";
                Prov.Estado = "Activo"; // Por defecto activo

                _logger.LogInformation($"Datos proveedor: {Prov.CedulaJuridica}, {Prov.NombreEmpresa}, Tipo: {Prov.TipoTransaccion}");

                var resp = await _almacen.ProcesarProveedorAsync(Prov);

                if (resp == null)
                {
                    _logger.LogError("Respuesta del servicio es NULL");
                    ModelState.AddModelError("", "El servicio no respondió.");
                    return Page();
                }

                _logger.LogInformation($"Respuesta WS: Resultado={resp.ResultadoOperacion}, Mensaje={resp.Mensaje}");

                if (!resp.ResultadoOperacion)
                {
                    ModelState.AddModelError("", "Error al realizar el proceso: " + resp.Mensaje);
                    return Page();
                }

                TempData["MensajeExitoProveedores"] = "¡Proceso finalizado de forma exitosa!";
                _logger.LogInformation("=== PROVEEDOR CREADO EXITOSAMENTE ===");

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Excepción al crear proveedor: {ex.Message}");
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return Page();
            }
        }
    }
}