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
        private readonly ILogger<EditModel> _logger;

        public EditModel(IAlmacenService almacen, ILogger<EditModel> logger)
        {
            _almacen = almacen;
            _logger = logger;
        }

        [BindProperty]
        public Producto Prod { get; set; } = new Producto();

        public IActionResult OnGet(string numero)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numero))
                {
                    return RedirectToPage("Index");
                }

                // Aquí deberías cargar el producto desde el servicio
                // Por ahora, solo establecemos el número
                Prod.NumeroProducto = numero;
                Prod.TipoTransaccion = "2"; // Para modificación

                _logger.LogInformation($"Editando producto: {numero}");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar producto: {ex.Message}");
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO MODIFICAR PRODUCTO ===");

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

                // CORRECCIÓN: El TipoTransaccion debe ser "2" para modificar
                Prod.TipoTransaccion = "2";

                _logger.LogInformation($"Modificando producto: {Prod.NumeroProducto}, Tipo: {Prod.TipoTransaccion}");

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
                _logger.LogInformation("=== PRODUCTO MODIFICADO EXITOSAMENTE ===");

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Excepción al modificar producto: {ex.Message}");
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return Page();
            }
        }
    }
}