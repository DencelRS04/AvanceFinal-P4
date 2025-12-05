namespace WebProducto.Models
{
    public class LoginRequest
    {
        public string Usuario { get; set; } = "";
        public string Contrasenia { get; set; } = "";
        public string ErrorMensaje { get; set; } = ""; // Agregar esta línea

    }
}
