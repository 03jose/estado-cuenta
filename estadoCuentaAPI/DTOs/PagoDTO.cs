namespace estadoCuentaAPI.DTOs
{
    public class PagoDTO
    {
        public int TarjetaCreditoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }

        public string Descripcion { get; set; }
    }
}
