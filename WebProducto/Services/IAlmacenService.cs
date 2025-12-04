using ServiceReferenceAlmacen;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public interface IAlmacenService
    {
        Task<Resultado> ProcesarProducto(Producto p);
        Task<Resultado> ProcesarProveedor(Proveedor p);
        Task<Resultado> ProcesarCompra(Compra c);
    }
}
