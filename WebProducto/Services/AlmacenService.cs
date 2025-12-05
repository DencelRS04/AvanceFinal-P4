using System.Threading.Tasks;
using ServiceReferenceAlmacen;

namespace WebProducto.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly ServiceClient _client;

        public AlmacenService(ServiceClient client)
        {
            _client = client;
        }

        public Task<Resultado> ProcesarProductoAsync(Producto producto)
        {
            // MÉTODO REAL DEL WS: ProcesarProductoAsync
            return _client.ProcesarProductoAsync(producto);
        }

        public Task<Resultado> ProcesarProveedorAsync(Proveedor proveedor)
        {
            // MÉTODO REAL DEL WS: ProcesarProveedorAsync
            return _client.ProcesarProveedorAsync(proveedor);
        }
    }
}
