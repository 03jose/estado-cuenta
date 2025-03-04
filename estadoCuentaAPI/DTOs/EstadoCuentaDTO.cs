namespace estadoCuentaAPI.DTOs
{
    public class EstadoCuentaDTO
    {

        public string NombreTitular { get; set; }
        public string NumeroTarjeta { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal SaldoUtilizado { get; set; }
        public List<MovimientoDTO> Movimientos { get; set; }
        public decimal MontoTotalComprasMesActual { get; set; }
        public decimal MontoTotalComprasMesAnterior { get; set; }
        public decimal InteresBonificable { get; set; }
        public decimal CuotaMinima { get; set; }
        public decimal MontoTotalPagar { get; set; }
        public decimal MontoContadoConIntereses { get; set; }
    }


}
