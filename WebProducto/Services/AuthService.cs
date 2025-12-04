using ServiceReferenceAuth;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public class AuthService : IAuthService
    {
        private readonly AutenticacionClient _client;

        public AuthService()
        {
            _client = new AutenticacionClient(
                AutenticacionClient.EndpointConfiguration.BasicHttpBinding_IAutenticacion
            );
        }

        public Task<AutenticacionResponse> Login(string usuario, string contrasenia, int tipo)
        {
            return _client.AutenticarUsuarioAsync(usuario, contrasenia, tipo);
        }
    }
}
