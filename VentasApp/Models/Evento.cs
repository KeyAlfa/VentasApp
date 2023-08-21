using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class Evento
    {
        public Evento()
        {
            Entrada = new HashSet<Entrada>();
            EventoCategoria = new HashSet<EventoCategoria>();
        }

        public int EventoId { get; set; }
        public string NombreEvento { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Ubicacion { get; set; } = null!;
        public DateTime Fecha { get; set; }

        public virtual ICollection<Entrada> Entrada { get; set; }
        public virtual ICollection<EventoCategoria> EventoCategoria { get; set; }
    }
}
