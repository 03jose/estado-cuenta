namespace estadoCuentaAPI.DTOs
{
    public class HistorialTransaccionesDTO
    {
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string TipoMovimiento { get; set; }
    }
}
