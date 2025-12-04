namespace Biblioteca.Models
{
    public class LoginRequest
    {
        public string Usuario { get; set; } = "";
        public string Clave { get; set; } = "";
        public string Tipo { get; set; } = "";  // administrador / cliente, etc.
    }
}
