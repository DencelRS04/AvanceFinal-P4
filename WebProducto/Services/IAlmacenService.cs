using SolucionORM.Model;

namespace WebProducto.Services
{
    public interface IAlmacenService
    {
        Task<List<ProductoDTO>> ObtenerProductos();
        Task<ProductoDTO> ObtenerProducto(int id);
        Task<RespuestaWS> GuardarProducto(ProductoDTO dto);
        Task<RespuestaWS> ModificarProducto(ProductoDTO dto);

        Task<List<ProveedorDTO>> ObtenerProveedores();
        Task<ProveedorDTO> ObtenerProveedor(int id);
        Task<RespuestaWS> GuardarProveedor(ProveedorDTO dto);
        Task<RespuestaWS> ModificarProveedor(ProveedorDTO dto);
        Task<RespuestaWS> InactivarProveedor(string cedula);
    }
}
