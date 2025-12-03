using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaORM.Model
{
    public class Producto
    {
        [Key]
        public int IDProducto { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
    }
}
