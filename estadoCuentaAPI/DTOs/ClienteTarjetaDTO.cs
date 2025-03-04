namespace estadoCuentaAPI.DTOs
{
    public class ClienteTarjetaDTO
    {
        public string NombreCompletoCliente { get; set; }

        public string NumeroTarjeta { get; set; }

        public int? ClienteId { get; set; }

        public int TarjetaCreditoId { get; set; }

    }
}
