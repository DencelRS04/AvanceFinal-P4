using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string Usuario { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string ErrorMensaje { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var resp = await _auth.Login(Usuario, Contrasenia, 1);

            if (!resp.Resultado)
            {
                ErrorMensaje = resp.Mensaje;
                return Page();
            }

            return RedirectToPage("/Shared/Index");
        }
    }
}
