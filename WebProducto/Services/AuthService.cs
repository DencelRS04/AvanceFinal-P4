using SolucionORM.Model;
using System.Net.Http.Json;

namespace WebProducto.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<RespuestaWS> Autenticar(LoginRequest request)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_AUTENTICACION1", request);
            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }
    }
}
