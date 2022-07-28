using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Ventum
    {
        public int IdVenta { get; set; }
        public decimal? Total { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdMetodoPago { get; set; }

        public virtual MetodoPago? IdMetodoPagoNavigation { get; set; }
    }
}
