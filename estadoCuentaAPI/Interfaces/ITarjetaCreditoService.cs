using estadoCuentaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Interfaces
{
    public interface ITarjetaCreditoService
    {

        Task<EstadoCuentaDTO> ObtenerEstadoCuentaAsync(int tarjetaCreditoId);
        Task<List<MovimientoDTO>> ObtenerHistorialTransaccionesAsync(int tarjetaCreditoId);

        Task<string> RegistrarPagoAsync(PagoDTO movimiento);
        //Task<string> RegistrarCompraAsync(MovimientoCompraDTO movimiento);
        Task<(bool Success, List<string> Errors, string Message)> RegistrarCompraAsync(MovimientoCompraDTO movimiento);

        Task<string> CrearRegistroTarjetaCredito(TarjetaCreditoDTO tarjetaCreditoDTO);

        Task<List<ClienteTarjetaDTO>> ObtenerListaTarjetas();

        Task<List<MovimientoDTO>> ObtenerComprasEnRangoAsync(int tarjetaCreditoId, DateTime fechaInicio, DateTime fechaFin);
        
    }
}

