using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProducto.Services;

namespace WebProducto.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IAlmacenService _almacen;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IAlmacenService almacen, ILogger<IndexModel> logger)
        {
            _almacen = almacen;
            _logger = logger;
        }

        // Lista de productos para mostrar en la tabla
        public List<Producto> Productos { get; set; } = new List<Producto>();

        // Filtro de búsqueda
        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        // Mensajes de éxito/error
        [TempData]
        public string MensajeExito { get; set; }

        [TempData]
        public string MensajeError { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO CARGA DE PRODUCTOS ===");
                _logger.LogInformation("Cargando página Index de Productos");

                // ========== OBTENER DATOS REALES DEL SERVICIO ==========
                _logger.LogInformation("Llamando a ObtenerProductosAsync()...");

                Productos = await _almacen.ObtenerProductosAsync();

                _logger.LogInformation($"Respuesta recibida: {Productos?.Count ?? 0} productos");

                // Si no hay datos, mostrar mensaje
                if (Productos == null || Productos.Count == 0)
                {
                    _logger.LogWarning("No se obtuvieron productos del servicio");
                    MensajeError = "No se encontraron productos en el sistema.";

                    // Mostrar datos de prueba SOLO si está en desarrollo
#if DEBUG
                    _logger.LogInformation("Modo DEBUG: Mostrando datos de prueba");
                    await CargarProductosDePrueba();
#endif
                }
                else
                {
                    _logger.LogInformation($"✅ Se cargaron {Productos.Count} productos reales de la base de datos");

                    // Mostrar algunos productos en el log para debugging
                    foreach (var producto in Productos.Take(3))
                    {
                        _logger.LogInformation($"  - {producto.NumeroProducto}: {producto.NombreProducto} - ${producto.Precio}");
                    }
                    if (Productos.Count > 3)
                    {
                        _logger.LogInformation($"  ... y {Productos.Count - 3} más");
                    }
                }

                // ========== APLICAR FILTRO SI EXISTE ==========
                if (!string.IsNullOrWhiteSpace(Filtro))
                {
                    _logger.LogInformation($"Aplicando filtro: '{Filtro}'");

                    Productos = Productos.Where(p =>
                        (p.NumeroProducto?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.NombreProducto?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.Precio.ToString().Contains(Filtro, StringComparison.OrdinalIgnoreCase))
                    ).ToList();

                    _logger.LogInformation($"Después del filtro: {Productos.Count} productos");
                }

                // ========== ORDENAR PRODUCTOS ==========
                Productos = Productos.OrderBy(p => p.NumeroProducto).ToList();

                _logger.LogInformation("=== FIN CARGA DE PRODUCTOS ===");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ ERROR al cargar productos: {ex.Message}");
                _logger.LogError($"StackTrace: {ex.StackTrace}");

                MensajeError = $"Error al cargar los productos: {ex.Message}";

                // En caso de error, mostrar datos de prueba
                await CargarProductosDePrueba();

                if (Productos.Count > 0)
                {
                    MensajeError += " (Mostrando datos de prueba)";
                }
            }
        }

        // ========== MÉTODO PARA DATOS DE PRUEBA (solo desarrollo) ==========
        private async Task CargarProductosDePrueba()
        {
            _logger.LogInformation("Cargando datos de prueba para productos");

            Productos = new List<Producto>
            {
                new Producto { NumeroProducto = "PROD-001", NombreProducto = "Laptop Dell XPS 13", Precio = 1299.99m },
                new Producto { NumeroProducto = "PROD-002", NombreProducto = "Mouse Logitech MX Master 3", Precio = 99.99m },
                new Producto { NumeroProducto = "PROD-003", NombreProducto = "Teclado Mecánico RGB", Precio = 149.99m },
                new Producto { NumeroProducto = "PROD-004", NombreProducto = "Monitor 27\" 4K", Precio = 399.99m },
                new Producto { NumeroProducto = "PROD-005", NombreProducto = "Disco SSD 1TB NVMe", Precio = 129.99m },
                new Producto { NumeroProducto = "PROD-006", NombreProducto = "Impresora HP LaserJet", Precio = 299.99m },
                new Producto { NumeroProducto = "PROD-007", NombreProducto = "Router WiFi 6", Precio = 199.99m },
                new Producto { NumeroProducto = "PROD-008", NombreProducto = "Webcam HD 1080p", Precio = 89.99m }
            };

            _logger.LogInformation($"Datos de prueba cargados: {Productos.Count} productos");

            await Task.CompletedTask;
        }
    }
}