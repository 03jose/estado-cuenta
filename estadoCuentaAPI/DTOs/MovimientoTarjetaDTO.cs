namespace estadoCuentaAPI.DTOs
{
    public class MovimientoTarjetaDTO
    {
        public int MovimientoTarjetaId { get; set; }
        public int? TarjetaCreditoId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal Monto { get; set; }
        public string TipoMovimiento { get; set; } = null!;
    }
}
