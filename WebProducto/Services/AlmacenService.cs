using ServiceReferenceAlmacen;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly ServiceClient _client;
        private readonly ILogger<AlmacenService> _logger;

        public AlmacenService(ServiceClient client, ILogger<AlmacenService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Resultado> ProcesarProductoAsync(Producto producto)
        {
            try
            {
                _logger.LogInformation($"Procesando producto: {producto.NumeroProducto}, TipoTransaccion: {producto.TipoTransaccion}");

                // Validar que el TipoTransaccion sea válido (1=Crear, 2=Modificar)
                if (string.IsNullOrWhiteSpace(producto.TipoTransaccion))
                {
                    _logger.LogWarning("TipoTransaccion vacío, usando valor por defecto (1)");
                    producto.TipoTransaccion = "1";
                }
                else if (producto.TipoTransaccion != "1" && producto.TipoTransaccion != "2")
                {
                    _logger.LogWarning($"TipoTransaccion inválido: {producto.TipoTransaccion}. Usando 1 por defecto.");
                    producto.TipoTransaccion = "1";
                }

                _logger.LogInformation($"Enviando producto al WS: {producto.NumeroProducto}, {producto.NombreProducto}, {producto.Precio}, Tipo: {producto.TipoTransaccion}");

                var resultado = await _client.ProcesarProductoAsync(producto);

                _logger.LogInformation($"Respuesta WS Producto: Resultado={resultado?.ResultadoOperacion}, Mensaje={resultado?.Mensaje}");

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ProcesarProductoAsync: {ex.Message}");
                return new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = $"Error de conexión: {ex.Message}"
                };
            }
        }

        public async Task<Resultado> ProcesarProveedorAsync(Proveedor proveedor)
        {
            try
            {
                _logger.LogInformation($"Procesando proveedor: {proveedor.CedulaJuridica}, TipoTransaccion: {proveedor.TipoTransaccion}");

                // Validar que el TipoTransaccion sea válido (3=Crear, 4=Modificar)
                if (string.IsNullOrWhiteSpace(proveedor.TipoTransaccion))
                {
                    _logger.LogWarning("TipoTransaccion vacío, usando valor por defecto (3)");
                    proveedor.TipoTransaccion = "3";
                }
                else if (proveedor.TipoTransaccion != "3" && proveedor.TipoTransaccion != "4")
                {
                    _logger.LogWarning($"TipoTransaccion inválido: {proveedor.TipoTransaccion}. Usando 3 por defecto.");
                    proveedor.TipoTransaccion = "3";
                }

                // Asegurar que el estado esté definido
                if (string.IsNullOrWhiteSpace(proveedor.Estado))
                {
                    proveedor.Estado = "Activo";
                }

                _logger.LogInformation($"Enviando proveedor al WS: {proveedor.CedulaJuridica}, {proveedor.NombreEmpresa}, Tipo: {proveedor.TipoTransaccion}");

                var resultado = await _client.ProcesarProveedorAsync(proveedor);

                _logger.LogInformation($"Respuesta WS Proveedor: Resultado={resultado?.ResultadoOperacion}, Mensaje={resultado?.Mensaje}");

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ProcesarProveedorAsync: {ex.Message}");
                return new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = $"Error de conexión: {ex.Message}"
                };
            }
        }

        public Task<Resultado> ProcesarCompraAsync(Compra compra)
        {
            return _client.ProcesarCompraAsync(compra);
        }
    }
}