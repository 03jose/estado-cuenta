using System;
using System.Collections.Generic;

namespace estadoCuentaAPI.Models
{
    public partial class MovimientoTarjetum
    {
        public int MovimientoTarjetaId { get; set; }
        public int? TarjetaCreditoId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal Monto { get; set; }
        public string TipoMovimiento { get; set; } = null!;

        public virtual TarjetaCredito? TarjetaCredito { get; set; }
    }
}
