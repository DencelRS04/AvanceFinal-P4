using SolucionORM.Model;
using System.Net.Http.Json;

namespace WebProducto.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly HttpClient _http;

        public AlmacenService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductoDTO>> ObtenerProductos()
        {
            return await _http.GetFromJsonAsync<List<ProductoDTO>>(
                "http://TU_WS/WS_ALMACEN1/listar"
            );
        }

        public async Task<ProductoDTO> ObtenerProducto(int id)
        {
            return await _http.GetFromJsonAsync<ProductoDTO>(
                $"http://TU_WS/WS_ALMACEN1/buscar?id={id}"
            );
        }

        public async Task<RespuestaWS> GuardarProducto(ProductoDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_ALMACEN1/guardar", dto);
            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }

        public async Task<RespuestaWS> ModificarProducto(ProductoDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_ALMACEN1/modificar", dto);
            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }

        // ----------------- PROVEEDORES -----------------

        public async Task<List<ProveedorDTO>> ObtenerProveedores()
        {
            return await _http.GetFromJsonAsync<List<ProveedorDTO>>(
                "http://TU_WS/WS_ALMACEN2/listar"
            );
        }

        public async Task<ProveedorDTO> ObtenerProveedor(int id)
        {
            return await _http.GetFromJsonAsync<ProveedorDTO>(
                $"http://TU_WS/WS_ALMACEN2/buscar?id={id}"
            );
        }

        public async Task<RespuestaWS> GuardarProveedor(ProveedorDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_ALMACEN2/guardar", dto);
            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }

        public async Task<RespuestaWS> ModificarProveedor(ProveedorDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_ALMACEN2/modificar", dto);
            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }

        public async Task<RespuestaWS> InactivarProveedor(string cedula)
        {
            var resp = await _http.PostAsJsonAsync("http://TU_WS/WS_ALMACEN2/inactivar",
                new { Cedula = cedula });

            return await resp.Content.ReadFromJsonAsync<RespuestaWS>();
        }
    }
}
