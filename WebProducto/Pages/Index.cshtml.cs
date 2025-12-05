using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebProducto.Services;

namespace WebProducto.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAutenticacionService _autenticacion;

        public IndexModel(IAutenticacionService autenticacion)
        {
            _autenticacion = autenticacion;
        }

        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string MensajeError { get; set; }

        public void OnGet()
        {
            // Limpiar sesión al entrar a login
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasenia))
            {
                MensajeError = "Debe digitar usuario y contraseña.";
                return Page();
            }

            var resp = await _autenticacion.AutenticarAdminAsync(Usuario, Contrasenia);

            if (!resp.Resultado || resp.Usuario == null || resp.Usuario.Estado != "Activo")
            {
                MensajeError = "Usuario y/o contraseña incorrectos.";
                return Page();
            }

            // Guardar usuario en sesión
            HttpContext.Session.SetString("UsuarioAdmin", resp.Usuario.Usuario);

            // Redirigir a ADM2: Productos
            return RedirectToPage("/Productos/Index");
        }
    }
}
