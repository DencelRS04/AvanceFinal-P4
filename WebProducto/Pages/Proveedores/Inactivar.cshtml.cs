using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblioteca.Models;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class InactivarModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public InactivarModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public ProveedorDTO Proveedor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Proveedor = await _almacen.ObtenerProveedor(id);

            if (Proveedor == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resp = await _almacen.InactivarProveedor(Proveedor.CedulaJuridica);

            if (resp.Exito)
                return RedirectToPage("./Index");

            ModelState.AddModelError("", resp.Mensaje);
            return Page();
        }
    }
}
