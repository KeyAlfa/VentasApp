using System;
using System.Collections.Generic;

namespace VentasApp.Models
{
    public partial class TarjetaCredito
    {
        public int TarjetaCreditoId { get; set; }
        public string UsuarioId { get; set; } = null!;
        public double Saldo { get; set; }

        public virtual AspNetUser Usuario { get; set; } = null!;
    }
}
