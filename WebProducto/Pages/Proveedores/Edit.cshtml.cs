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
        private readonly ILogger<EditModel> _logger;

        public EditModel(IAlmacenService almacen, ILogger<EditModel> logger)
        {
            _almacen = almacen;
            _logger = logger;
        }

        [BindProperty]
        public Proveedor Prov { get; set; } = new Proveedor();

        public IActionResult OnGet(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula))
                {
                    return RedirectToPage("Index");
                }

                // Aquí deberías cargar el proveedor desde el servicio
                // Por ahora, solo establecemos la cédula
                Prov.CedulaJuridica = cedula;
                Prov.TipoTransaccion = "4"; // Para modificación
                Prov.Estado = "Activo"; // Valor por defecto

                _logger.LogInformation($"Editando proveedor: {cedula}");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar proveedor: {ex.Message}");
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO MODIFICAR PROVEEDOR ===");

                if (string.IsNullOrWhiteSpace(Prov.CedulaJuridica) ||
                    string.IsNullOrWhiteSpace(Prov.NombreEmpresa) ||
                    string.IsNullOrWhiteSpace(Prov.NombreContacto) ||
                    string.IsNullOrWhiteSpace(Prov.Telefono) ||
                    string.IsNullOrWhiteSpace(Prov.Correo))
                {
                    ModelState.AddModelError("", "Todos los campos son obligatorios.");
                    return Page();
                }

                // CORRECCIÓN CRÍTICA: El TipoTransaccion debe ser "4" para modificar
                Prov.TipoTransaccion = "4";

                // Si no se especifica estado, mantener como Activo
                if (string.IsNullOrWhiteSpace(Prov.Estado))
                {
                    Prov.Estado = "Activo";
                }

                _logger.LogInformation($"Modificando proveedor: {Prov.CedulaJuridica}, Tipo: {Prov.TipoTransaccion}");

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

                TempData["MensajeExitoProveedores"] = "¡Modificación finalizada de forma exitosa!";
                _logger.LogInformation("=== PROVEEDOR MODIFICADO EXITOSAMENTE ===");

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Excepción al modificar proveedor: {ex.Message}");
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return Page();
            }
        }
    }
}