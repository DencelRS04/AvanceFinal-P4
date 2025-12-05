using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAutenticacion;

namespace WebProducto.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAutenticacionService _auth;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasenia { get; set; } = string.Empty;

        public string ErrorMensaje { get; set; } = string.Empty;

        public LoginModel(IAutenticacionService auth, ILogger<LoginModel> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        public void OnGet()
        {
            // Limpiar sesión si viene del logout
            HttpContext.Session.Clear();
            TempData.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("=== INICIO PROCESO LOGIN ===");

                if (string.IsNullOrWhiteSpace(Usuario) ||
                    string.IsNullOrWhiteSpace(Contrasenia))
                {
                    ErrorMensaje = "Debe digitar usuario y contraseña.";
                    return Page();
                }

                _logger.LogInformation($"Usuario intentando login: {Usuario}");

                // ========== CREDENCIALES DEMO (PARA DESARROLLO) ==========
                if (Usuario == "deroso" && Contrasenia == "11111111111aA-")
                {
                    _logger.LogInformation("Usando credenciales DEMO para desarrollo");

                    TempData["UsuarioLogueado"] = "Administrador Demo";
                    HttpContext.Session.SetString("UsuarioActual", Usuario);
                    HttpContext.Session.SetString("NombreCompleto", "Administrador Demo");
                    HttpContext.Session.SetInt32("TipoUsuario", 1); // 1 = Administrador

                    _logger.LogInformation("Login DEMO exitoso, redirigiendo a /Productos/Index");
                    return RedirectToPage("/Productos/Index");
                }

                // ========== AUTENTICACIÓN REAL CON WCF ==========
                _logger.LogInformation("Intentando autenticación con servicio WCF...");

                var resp = await _auth.AutenticarAdminAsync(Usuario, Contrasenia);

                _logger.LogInformation($"Respuesta WCF recibida");
                _logger.LogInformation($"  ¿Respuesta null? {resp == null}");

                if (resp != null)
                {
                    _logger.LogInformation($"  Resultado: {resp.Resultado}");
                    _logger.LogInformation($"  Mensaje: {resp.Mensaje}");
                    _logger.LogInformation($"  Usuario object null? {resp.Usuario == null}");

                    // Debug detallado del objeto Usuario si existe
                    if (resp.Usuario != null)
                    {
                        _logger.LogInformation($"  DATOS USUARIO RECIBIDOS:");
                        _logger.LogInformation($"    Id: {resp.Usuario.Identificacion}");
                        _logger.LogInformation($"    Nombre: {resp.Usuario.Nombre}");
                        _logger.LogInformation($"    PrimerApellido: {resp.Usuario.PrimerApellido}");
                        _logger.LogInformation($"    Correo: {resp.Usuario.Correo}");
                        _logger.LogInformation($"    Usuario: {resp.Usuario.Usuario}");
                        _logger.LogInformation($"    Estado: {resp.Usuario.Estado}");
                        _logger.LogInformation($"    Tipo: {resp.Usuario.Tipo}");
                    }
                    else
                    {
                        _logger.LogWarning("  ⚠️ Usuario object es NULL en la respuesta");
                    }
                }
                else
                {
                    _logger.LogError("  ❌ Respuesta completa es NULL");
                }

                // ========== VALIDACIÓN DE RESPUESTA ==========
                if (resp == null)
                {
                    ErrorMensaje = "El servicio de autenticación no respondió. Intente con credenciales DEMO.";
                    _logger.LogError("Servicio WCF no respondió (respuesta null)");
                    return Page();
                }

                if (!resp.Resultado)
                {
                    ErrorMensaje = "Credenciales incorrectas: " + resp.Mensaje;
                    _logger.LogWarning($"Credenciales rechazadas: {resp.Mensaje}");
                    return Page();
                }

                // ========== MANEJO DE USUARIO NULL CON RESULTADO TRUE ==========
                if (resp.Usuario == null)
                {
                    _logger.LogWarning("⚠️ Respuesta exitosa pero Usuario es null. Usando workaround.");

                    // WORKAROUND: Para permitir desarrollo mientras se arregla el WCF
                    TempData["UsuarioLogueado"] = Usuario;
                    HttpContext.Session.SetString("UsuarioActual", Usuario);
                    HttpContext.Session.SetString("NombreCompleto", Usuario);
                    HttpContext.Session.SetInt32("TipoUsuario", 1);

                    _logger.LogInformation($"Workaround aplicado. Redirigiendo a /Productos/Index");
                    return RedirectToPage("/Productos/Index");
                }

                // ========== LOGIN EXITOSO (RESPUESTA COMPLETA) ==========
                var nombreCompleto = $"{resp.Usuario.Nombre} {resp.Usuario.PrimerApellido}";

                TempData["UsuarioLogueado"] = nombreCompleto;
                HttpContext.Session.SetString("UsuarioActual", resp.Usuario.Usuario);
                HttpContext.Session.SetString("NombreCompleto", nombreCompleto);
                HttpContext.Session.SetString("UserId", resp.Usuario.Identificacion);
                HttpContext.Session.SetInt32("TipoUsuario", resp.Usuario.Tipo);

                _logger.LogInformation($"✅ Login exitoso para: {nombreCompleto}");
                _logger.LogInformation($"=== FIN PROCESO LOGIN ===");

                return RedirectToPage("/Productos/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ EXCEPCIÓN en login: {ex.Message}");
                _logger.LogError($"StackTrace: {ex.StackTrace}");

                ErrorMensaje = $"Error de conexión con el servicio: {ex.Message}";

                // Si hay error de conexión, sugerir usar demo
                if (ex.Message.Contains("No se pudo conectar") ||
                    ex.Message.Contains("timeout") ||
                    ex.Message.Contains("endpoint"))
                {
                    ErrorMensaje += " Use las credenciales DEMO para continuar con el desarrollo.";
                }

                return Page();
            }
        }
    }
}