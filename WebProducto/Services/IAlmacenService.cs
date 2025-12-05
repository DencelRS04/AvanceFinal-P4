using ServiceReferenceAlmacen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public interface IAlmacenService
    {
        // ADM2 – Productos
        Task<Resultado> ProcesarProductoAsync(Producto producto);

        // ADM3 – Proveedores
        Task<Resultado> ProcesarProveedorAsync(Proveedor proveedor);

        // ADM4 – Compras
        Task<Resultado> ProcesarCompraAsync(Compra compra);

        // Métodos de consulta
        Task<List<Producto>> ObtenerProductosAsync();
        Task<List<Proveedor>> ObtenerProveedoresAsync();
    }
}