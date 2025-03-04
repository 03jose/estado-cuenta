namespace estadoCuentaAPI.DTOs
{
    public class MovimientoDTO
    {
        public DateTime FechaMoviomiento { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }

        public string TipoMovimiento { get; set; } = null!;
    }
}
