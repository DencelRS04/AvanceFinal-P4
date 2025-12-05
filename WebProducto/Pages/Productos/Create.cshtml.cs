using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Threading.Tasks;
using WebProducto.Services;

namespace WebProducto.Pages.Productos
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
        public Producto Prod { get; set; } = new Producto();

        public void OnGet()
        {
            _logger.LogInformation("Página Create/Productos cargada");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO CREAR PRODUCTO ===");

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

                // CORRECCIÓN: El TipoTransaccion debe ser "1" para crear
                Prod.TipoTransaccion = "1";

                _logger.LogInformation($"Datos producto: {Prod.NumeroProducto}, {Prod.NombreProducto}, {Prod.Precio}, Tipo: {Prod.TipoTransaccion}");

                var resp = await _almacen.ProcesarProductoAsync(Prod);

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

                TempData["MensajeExitoProductos"] = "¡Proceso finalizado de forma exitosa!";
                _logger.LogInformation("=== PRODUCTO CREADO EXITOSAMENTE ===");

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Excepción al crear producto: {ex.Message}");
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return Page();
            }
        }
    }
}