using Microsoft.EntityFrameworkCore;

namespace estadoCuentaAPI.Models
{
    [Keyless] // <-- Esto indica que no es una tabla real en la BD
    public class ClienteTarjetaView
    {
        public int ClienteId { get; set; }
        public int TarjetaCreditoId { get; set; }
        public string NumeroTarjeta { get; set; }
        public string NombreCompletoCliente { get; set; }
    }
}
