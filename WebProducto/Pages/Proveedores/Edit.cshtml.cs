using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class EditModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public EditModel(IAlmacenService almacen)
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
            var resp = await _almacen.ModificarProveedor(Proveedor);

            if (resp.Exito)
                return RedirectToPage("./Index");

            ModelState.AddModelError("", resp.Mensaje);
            return Page();
        }
    }
}
