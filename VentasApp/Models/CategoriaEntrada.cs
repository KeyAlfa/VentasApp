using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class CategoriaEntrada
    {
        public CategoriaEntrada()
        {
            EventoCategoria = new HashSet<EventoCategoria>();
        }

        public int CategoriaEntradaId { get; set; }
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public int Capacidad { get; set; }

        public virtual ICollection<EventoCategoria> EventoCategoria { get; set; }
    }
}
