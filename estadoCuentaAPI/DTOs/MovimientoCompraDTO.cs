namespace estadoCuentaAPI.DTOs
{
    public class MovimientoCompraDTO
    {
        public int TarjetaCreditoId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
