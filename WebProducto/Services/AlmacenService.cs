using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceReferenceAlmacen;

namespace WebProducto.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly ServiceClient _client;
        private readonly ILogger<AlmacenService> _logger;

        public AlmacenService(ILogger<AlmacenService> logger)
        {
            _logger = logger;

            // Crear cliente WCF
            _client = new ServiceClient(
                ServiceClient.EndpointConfiguration.BasicHttpBinding_IService
            );

            // Configurar timeouts
            _client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            _client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
        }

        public async Task<Resultado> ProcesarProductoAsync(Producto producto)
        {
            try
            {
                _logger.LogInformation("Procesando producto: " + producto.NumeroProducto + ", Tipo: " + producto.TipoTransaccion);

                // Validar que el TipoTransaccion sea válido (1=Crear, 2=Modificar)
                if (string.IsNullOrEmpty(producto.TipoTransaccion))
                {
                    _logger.LogWarning("TipoTransaccion vacío, usando valor por defecto (1)");
                    producto.TipoTransaccion = "1";
                }
                else if (producto.TipoTransaccion != "1" && producto.TipoTransaccion != "2")
                {
                    _logger.LogWarning("TipoTransaccion inválido: " + producto.TipoTransaccion + ". Usando 1 por defecto.");
                    producto.TipoTransaccion = "1";
                }

                _logger.LogInformation("Enviando producto al WS: " + producto.NumeroProducto + ", " + producto.NombreProducto + ", " + producto.Precio + ", Tipo: " + producto.TipoTransaccion);

                var resultado = await _client.ProcesarProductoAsync(producto);

                _logger.LogInformation("Respuesta WS Producto: Resultado=" + (resultado != null ? resultado.ResultadoOperacion.ToString() : "null") + ", Mensaje=" + (resultado != null ? resultado.Mensaje : "null"));

                return resultado ?? new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "No se recibió respuesta del servicio"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en ProcesarProductoAsync: " + ex.Message);
                return new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "Error de conexión: " + ex.Message
                };
            }
        }

        public async Task<Resultado> ProcesarProveedorAsync(Proveedor proveedor)
        {
            try
            {
                _logger.LogInformation("Procesando proveedor: " + proveedor.CedulaJuridica + ", Tipo: " + proveedor.TipoTransaccion);

                // Validar que el TipoTransaccion sea válido (3=Crear, 4=Modificar)
                if (string.IsNullOrEmpty(proveedor.TipoTransaccion))
                {
                    _logger.LogWarning("TipoTransaccion vacío, usando valor por defecto (3)");
                    proveedor.TipoTransaccion = "3";
                }
                else if (proveedor.TipoTransaccion != "3" && proveedor.TipoTransaccion != "4")
                {
                    _logger.LogWarning("TipoTransaccion inválido: " + proveedor.TipoTransaccion + ". Usando 3 por defecto.");
                    proveedor.TipoTransaccion = "3";
                }

                // Asegurar que el estado esté definido
                if (string.IsNullOrEmpty(proveedor.Estado))
                {
                    proveedor.Estado = "Activo";
                }

                _logger.LogInformation("Enviando proveedor al WS: " + proveedor.CedulaJuridica + ", " + proveedor.NombreEmpresa + ", Tipo: " + proveedor.TipoTransaccion);

                var resultado = await _client.ProcesarProveedorAsync(proveedor);

                _logger.LogInformation("Respuesta WS Proveedor: Resultado=" + (resultado != null ? resultado.ResultadoOperacion.ToString() : "null") + ", Mensaje=" + (resultado != null ? resultado.Mensaje : "null"));

                return resultado ?? new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "No se recibió respuesta del servicio"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en ProcesarProveedorAsync: " + ex.Message);
                return new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "Error de conexión: " + ex.Message
                };
            }
        }

        public async Task<Resultado> ProcesarCompraAsync(Compra compra)
        {
            try
            {
                _logger.LogInformation("Procesando compra: " + compra.NumeroIngreso);
                var resultado = await _client.ProcesarCompraAsync(compra);
                return resultado ?? new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "No se recibió respuesta del servicio"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en ProcesarCompraAsync: " + ex.Message);
                return new Resultado
                {
                    ResultadoOperacion = false,
                    Mensaje = "Error de conexión: " + ex.Message
                };
            }
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo productos desde WS...");

                // Verificar conexión
                if (_client.State != System.ServiceModel.CommunicationState.Opened)
                {
                    await _client.OpenAsync();
                }

                // Llamar al método del servicio
                var productosArray = await _client.ObtenerProductosAsync();

                if (productosArray != null)
                {
                    var productos = productosArray.ToList();
                    _logger.LogInformation($"Se obtuvieron {productos.Count} productos del servicio");
                    return productos;
                }

                _logger.LogWarning("El servicio retornó null para productos");
                return new List<Producto>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener productos: {ex.Message}");

                // Datos de prueba si falla
                return new List<Producto>
                {
                    new Producto
                    {
                        NumeroProducto = "PROD-001",
                        NombreProducto = "Laptop Dell XPS 13",
                        Precio = 1299.99m,
                        TipoTransaccion = ""
                    },
                    new Producto
                    {
                        NumeroProducto = "PROD-002",
                        NombreProducto = "Mouse Logitech",
                        Precio = 49.99m,
                        TipoTransaccion = ""
                    },
                    new Producto
                    {
                        NumeroProducto = "PROD-003",
                        NombreProducto = "Teclado RGB",
                        Precio = 89.99m,
                        TipoTransaccion = ""
                    }
                };
            }
        }

        public async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo proveedores desde WS...");

                // Verificar conexión
                if (_client.State != System.ServiceModel.CommunicationState.Opened)
                {
                    await _client.OpenAsync();
                }

                // Llamar al método del servicio
                var proveedoresArray = await _client.ObtenerProveedoresAsync();

                if (proveedoresArray != null)
                {
                    var proveedores = proveedoresArray.ToList();
                    _logger.LogInformation($"Se obtuvieron {proveedores.Count} proveedores del servicio");
                    return proveedores;
                }

                _logger.LogWarning("El servicio retornó null para proveedores");
                return new List<Proveedor>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedores: {ex.Message}");

                // Datos de prueba si falla
                return new List<Proveedor>
                {
                    new Proveedor
                    {
                        CedulaJuridica = "3-101-000001",
                        NombreEmpresa = "Tecnología Nova S.A.",
                        NombreContacto = "Ana Rodríguez",
                        Telefono = "2222-1111",
                        Correo = "ventas@novatech.com",
                        Estado = "Activo",
                        TipoTransaccion = ""
                    },
                    new Proveedor
                    {
                        CedulaJuridica = "3-101-000002",
                        NombreEmpresa = "Suministros Industriales Costa Rica",
                        NombreContacto = "Carlos Méndez",
                        Telefono = "2555-2222",
                        Correo = "info@suministroscr.com",
                        Estado = "Activo",
                        TipoTransaccion = ""
                    }
                };
            }
        }
    }
}