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
    public class IndexModel : PageModel
    {
        private readonly BibliotecaORM.Model.Ambiente _context;

        public IndexModel(BibliotecaORM.Model.Ambiente context)
        {
            _context = context;
        }

        public IList<Producto> Producto { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Producto = await _context.Producto.ToListAsync();
        }
    }
}
