using System;
using System.Collections.Generic;

namespace estadoCuentaAPI.Models
{
    public partial class TarjetaCredito
    {
        public TarjetaCredito()
        {
            MovimientoTarjeta = new HashSet<MovimientoTarjetum>();
        }

        public int TarjetaCreditoId { get; set; }
        public int? ClienteId { get; set; }
        public string NumeroTarjeta { get; set; } = null!;
        public decimal LimiteCredito { get; set; }
        public decimal SaldoUtilizado { get; set; }
        public decimal TasaInteresConfigurable { get; set; }
        public DateTime FechaCorte { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal PorcentajeSaldoMin { get; set; }


        public virtual Cliente? Cliente { get; set; }
        public virtual ICollection<MovimientoTarjetum> MovimientoTarjeta { get; set; }
    }
}
