using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProducto.Services;

namespace WebProducto.Pages.Proveedores
{
    public class CrearModel : PageModel
    {
        private readonly IAlmacenService _almacen;

        public CrearModel(IAlmacenService almacen)
        {
            _almacen = almacen;
        }

        [BindProperty]
        public ProveedorDTO Proveedor { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            Proveedor.Estado = "Activo";

            var resp = await _almacen.GuardarProveedor(Proveedor);

            if (resp.Exito)
                return RedirectToPage("./Index");

            ModelState.AddModelError("", resp.Mensaje);
            return Page();
        }
    }
}
