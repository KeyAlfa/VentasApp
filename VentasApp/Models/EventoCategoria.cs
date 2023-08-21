using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class EventoCategoria
    {
        public int EventoCategoriaId { get; set; }
        public int CategoriaEntradaId { get; set; }
        public int EventoId { get; set; }

        public virtual CategoriaEntrada CategoriaEntrada { get; set; } = null!;
        public virtual Evento Evento { get; set; } = null!;
    }
}
