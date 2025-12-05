using System.Threading.Tasks;
using ServiceReferenceAlmacen;

namespace WebProducto.Services
{
    public interface IAlmacenService
    {
        // ADM2 – Productos
        Task<Resultado> ProcesarProductoAsync(Producto producto);

        // ADM3 – Proveedores
        Task<Resultado> ProcesarProveedorAsync(Proveedor proveedor);
    }
}
