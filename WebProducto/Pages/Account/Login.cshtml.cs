using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;
using ServiceReferenceAuth;

namespace WebProducto.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAutenticacionService _auth;

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasenia { get; set; } = string.Empty;

        public string ErrorMensaje { get; set; } = string.Empty;

        public LoginModel(IAutenticacionService auth)
        {
            _auth = auth;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Usuario) ||
                string.IsNullOrWhiteSpace(Contrasenia))
            {
                ErrorMensaje = "Debe digitar usuario y contraseña.";
                return Page();
            }

            // MÉTODO REAL DEL WS
            var resp = await _auth.AutenticarAdminAsync(Usuario, Contrasenia);

            if (resp == null || !resp.Resultado || resp.Usuario == null)
            {
                ErrorMensaje = "Usuario y/o contraseña incorrectos";
                return Page();
            }

            TempData["UsuarioLogueado"] =
                resp.Usuario.Nombre + " " + resp.Usuario.PrimerApellido;

            return RedirectToPage("/ADM/Menu");
        }
    }
}
