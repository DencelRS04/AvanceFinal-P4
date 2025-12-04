using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceAlmacen;
using WebProducto.Services;

namespace WebProducto.Pages.Productos
{
    public class DeleteModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public DeleteModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto();

        public IActionResult OnGet(string numero, string nombre, decimal precio)
        {
            Producto.NumeroProducto = numero;
            Producto.NombreProducto = nombre;
            Producto.Precio = precio;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Producto.TipoTransaccion = "2"; // WS espera 1=Agregar, 2=Modificar

            var resp = await _almacen.ProcesarProducto(Producto);

            if (!resp.ResultadoOperacion)
            {
                ModelState.AddModelError("", resp.Mensaje);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
