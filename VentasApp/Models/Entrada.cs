using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class Entrada
    {
        public int EntradaId { get; set; }
        public int EventoId { get; set; }
        public string UsuarioId { get; set; } = null!;

        public virtual Evento Evento { get; set; } = null!;
        public virtual AspNetUser Usuario { get; set; } = null!;
    }
}
