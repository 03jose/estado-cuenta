namespace estadoCuentaAPI.DTOs
{
    public class TarjetaCreditoDTO
    {
        public int TarjetaCreditoId { get; set; }
        public int? ClienteId { get; set; }
        public string NumeroTarjeta { get; set; } = null!;
        public decimal LimiteCredito { get; set; }
        public decimal SaldoUtilizado { get; set; }
        public decimal TasaInteres { get; set; }
        public DateTime FechaCorte { get; set; }
        public DateTime FechaPago { get; set; }

        public decimal SaldoDisponible => LimiteCredito - SaldoUtilizado; // Se calcula directamente
    }
}
