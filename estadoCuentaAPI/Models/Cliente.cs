using System;
using System.Collections.Generic;

namespace estadoCuentaAPI.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            TarjetaCreditos = new HashSet<TarjetaCredito>();
        }

        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;

        public string DUI { get; set; } = null!;

        public virtual ICollection<TarjetaCredito> TarjetaCreditos { get; set; }
    }
}
