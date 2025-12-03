using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BibliotecaORM.Model;

namespace WebProducto.Pages.productos
{
    public class DetailsModel : PageModel
    {
        private readonly BibliotecaORM.Model.Ambiente _context;

        public DetailsModel(BibliotecaORM.Model.Ambiente context)
        {
            _context = context;
        }

        public Producto Producto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FirstOrDefaultAsync(m => m.IDProducto == id);

            if (producto is not null)
            {
                Producto = producto;

                return Page();
            }

            return NotFound();
        }
    }
}
