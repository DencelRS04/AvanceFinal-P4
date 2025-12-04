using ServiceReferenceAlmacen;
using System.Threading.Tasks;

namespace WebProducto.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly ServiceClient _client;

        public AlmacenService()
        {
            _client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService);
        }

        public async Task<Resultado> ProcesarProducto(Producto p)
        {
            return await _client.ProcesarProductoAsync(p);
        }

        public async Task<Resultado> ProcesarProveedor(Proveedor p)
        {
            return await _client.ProcesarProveedorAsync(p);
        }

        public async Task<Resultado> ProcesarCompra(Compra c)
        {
            return await _client.ProcesarCompraAsync(c);
        }
    }
}
