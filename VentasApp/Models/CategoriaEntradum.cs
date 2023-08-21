using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class CategoriaEntradum
    {
        public CategoriaEntradum()
        {
            EventoCategoria = new HashSet<EventoCategorium>();
        }

        public int CategoriaEntradaId { get; set; }
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public int Capacidad { get; set; }

        public virtual ICollection<EventoCategorium> EventoCategoria { get; set; }
    }
}
