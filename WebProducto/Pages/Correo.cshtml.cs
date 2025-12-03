using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace WebProducto.Pages
{
    public class CorreoModel : PageModel
    {

        [BindProperty]
        public string Asunto { get; set; }

        [BindProperty]
        public string Correo { get; set; }

        [BindProperty]
        public string Contenido { get; set; }

        [BindProperty]
        public string MensajeProcesado { get; set; }


        public void OnGet()
        {
        }

        public void OnPost()
        {

            MailAddress addressFrom = new MailAddress("dencel31012004rs@gmail.com", "Dencel Rodriguez Solano");
            MailAddress addresto = new MailAddress(Correo);
            MailMessage mensaje = new MailMessage(addressFrom, addresto);
            mensaje.Subject = Asunto;
            mensaje.IsBodyHtml = true;
            mensaje.Body = Contenido;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            //app password
            smtp.Credentials = new NetworkCredential("dencel31012004rs@gmail.com", "stxv fsvg ppne mwmz\r\n");

            try
            {
                smtp.Send(mensaje);
                // Aqu  llega la informaci n enviada por el formulario
                MensajeProcesado = $"Datos enviados correctamente. Correo: {Correo}, Asunto: {Asunto}";
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceInformation(ex.ToString());
                MensajeProcesado = $"Error: {ex.ToString()}";
            }
        }
    }
}
