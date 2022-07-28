using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public decimal Total { get; set; }
        public string Fecha { get; set; }
        public ML.MetodoPago MetodoPago { get; set; }
    }
}
