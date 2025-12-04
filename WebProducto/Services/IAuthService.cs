using ServiceReferenceAuth;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public interface IAuthService
    {
        Task<AutenticacionResponse> Login(string usuario, string contrasenia, int tipo);
    }
}
