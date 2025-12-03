using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaORM.Model
{
    public partial class Ambiente: DbContext
    {
        public Ambiente() { }

        public Ambiente(DbContextOptions<Ambiente> options)
       : base(options)
        {
        }

        public virtual DbSet<Producto> Producto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder
                                                    optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.IDProducto).HasColumnName("IDProducto");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(70)
                    .IsUnicode(false);
                entity.Property(p => p.Precio)
                    .HasColumnType("int");
                //  entity.Property(e => e.Precio).HasDefaultValue(0);
            });

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
