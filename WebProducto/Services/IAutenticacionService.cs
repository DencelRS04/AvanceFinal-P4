using ServiceReferenceAuth;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public interface IAutenticacionService
    {
        Task<AutenticacionResponse> AutenticarAdminAsync(string usuario, string contraseña);
    }
}
