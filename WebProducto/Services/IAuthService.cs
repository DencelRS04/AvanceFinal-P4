using SolucionORM.Model;

namespace WebProducto.Services
{
    public interface IAuthService
    {
        Task<RespuestaWS> Autenticar(LoginRequest request);
    }
}
