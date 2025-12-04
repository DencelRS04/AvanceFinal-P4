using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BibliotecaORM.Model;
using WebProducto.Services;

namespace WebProducto.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _auth;

        public LoginModel(IAuthService auth)
        {
            _auth = auth;
        }

        [BindProperty]
        public LoginRequest Credenciales { get; set; }

        public string Error { get; set; }

        public void OnGet()
        {
            Credenciales = new LoginRequest();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Credenciales.Tipo = "administrador";

            var resp = await _auth.Autenticar(Credenciales);

            if (resp.Exito)
                return RedirectToPage("/Productos/Index");

            Error = "Usuario y/o contraseña incorrectos";
            return Page();
        }
    }
}
