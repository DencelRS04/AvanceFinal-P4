using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace WebProducto.Pages
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Contraseña { get; set; }

        [BindProperty]
        public string MensajeProcesado { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            try
            {
                // Aqui llega la informacion enviada por el formulario
                MensajeProcesado = $"Datos enviados correctamente. Usuario: {Usuario}, Contraseña: {Contraseña}";
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceInformation(ex.ToString());
                MensajeProcesado = $"Error: {ex.ToString()}";
            }
        }
}
}
