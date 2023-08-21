using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class Evento
    {
        public Evento()
        {
            Entrada = new HashSet<Entradum>();
            EventoCategoria = new HashSet<EventoCategorium>();
        }

        public int EventoId { get; set; }
        public string NombreEvento { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Ubicacion { get; set; } = null!;
        public DateTime Fecha { get; set; }

        public virtual ICollection<Entradum> Entrada { get; set; }
        public virtual ICollection<EventoCategorium> EventoCategoria { get; set; }
    }
}
