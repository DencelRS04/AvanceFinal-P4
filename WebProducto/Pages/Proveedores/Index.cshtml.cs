using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public IndexModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        public List<Proveedor> ListaProveedores { get; set; } = new();

        public async Task OnGetAsync()
        {
            // WS NO TIENE GET → creamos transacción CONSULTAR
            var consulta = new Proveedor
            {
                TipoTransaccion = "CONSULTAR"
            };

            var resultado = await _almacen.ProcesarProveedorAsync(consulta);

            if (resultado == null || !resultado.ResultadoOperacion)
            {
                ListaProveedores = new List<Proveedor>();
                return;
            }

            // Si tu WS retorna proveedores en otra estructura, ajustamos después
            // Aquí asumimos que devuelve ListaUsuarios o similar
        }
    }
}
