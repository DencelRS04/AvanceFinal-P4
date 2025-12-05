using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
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

        public List<Proveedor> Proveedores { get; set; } = new List<Proveedor>();

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        [TempData]
        public string MensajeExito { get; set; }

        [TempData]
        public string MensajeError { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO CARGA DE PROVEEDORES ===");
                _logger.LogInformation("Cargando página Index de Proveedores");

                // ========== OBTENER DATOS REALES DEL SERVICIO ==========
                _logger.LogInformation("Llamando a ObtenerProveedoresAsync()...");

                Proveedores = await _almacen.ObtenerProveedoresAsync();

                _logger.LogInformation($"Respuesta recibida: {Proveedores?.Count ?? 0} proveedores");

                if (Proveedores == null || Proveedores.Count == 0)
                {
                    _logger.LogWarning("No se obtuvieron proveedores del servicio");
                    MensajeError = "No se encontraron proveedores en el sistema.";

#if DEBUG
                    _logger.LogInformation("Modo DEBUG: Mostrando datos de prueba");
                    await CargarProveedoresDePrueba();
#endif
                }
                else
                {
                    _logger.LogInformation($"✅ Se cargaron {Proveedores.Count} proveedores reales");

                    // Convertir estados numéricos a texto para mostrar
                    foreach (var prov in Proveedores)
                    {
                        prov.Estado = prov.Estado == "1" ? "Activo" :
                                     (prov.Estado == "0" ? "Inactivo" : prov.Estado);
                    }
                }

                // ========== APLICAR FILTRO ==========
                if (!string.IsNullOrWhiteSpace(Filtro))
                {
                    Proveedores = Proveedores.Where(p =>
                        (p.CedulaJuridica?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.NombreEmpresa?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.NombreContacto?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.Telefono?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.Correo?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.Estado?.Contains(Filtro, StringComparison.OrdinalIgnoreCase) ?? false)
                    ).ToList();
                }

                // ========== ORDENAR ==========
                Proveedores = Proveedores.OrderBy(p => p.CedulaJuridica).ToList();

                _logger.LogInformation("=== FIN CARGA DE PROVEEDORES ===");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ ERROR al cargar proveedores: {ex.Message}");
                MensajeError = $"Error al cargar los proveedores: {ex.Message}";

                await CargarProveedoresDePrueba();
            }
        }

        private async Task CargarProveedoresDePrueba()
        {
            _logger.LogInformation("Cargando datos de prueba para proveedores");

            Proveedores = new List<Proveedor>
            {
                new Proveedor
                {
                    CedulaJuridica = "3-101-000001",
                    NombreEmpresa = "Tecnología Nova S.A.",
                    NombreContacto = "Ana Rodríguez",
                    Telefono = "2222-1111",
                    Correo = "ventas@novatech.com",
                    Estado = "Activo"
                },
                new Proveedor
                {
                    CedulaJuridica = "3-101-000002",
                    NombreEmpresa = "Suministros Industriales CR",
                    NombreContacto = "Carlos Méndez",
                    Telefono = "2555-2222",
                    Correo = "info@suministroscr.com",
                    Estado = "Activo"
                },
                new Proveedor
                {
                    CedulaJuridica = "3-101-000003",
                    NombreEmpresa = "Distribuidora Central",
                    NombreContacto = "María Fernández",
                    Telefono = "2444-3333",
                    Correo = "contacto@distcentral.com",
                    Estado = "Inactivo"
                }
            };

            await Task.CompletedTask;
        }
    }
}