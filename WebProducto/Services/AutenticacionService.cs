using ServiceReferenceAuth;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly AutenticacionClient _client;

        public AutenticacionService(AutenticacionClient client)
        {
            _client = client;
        }

        public Task<AutenticacionResponse> AutenticarAdminAsync(string usuario, string contrasenia)
        {
            return _client.AutenticarUsuarioAsync(usuario, contrasenia, 1);
        }
    }
}
