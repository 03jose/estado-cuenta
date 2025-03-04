using estadoCuentaAPI.DTOs;

namespace estadoCuentaAPI.Interfaces
{
    public interface IMovimientoService
    {
       Task< List<MovimientoTarjetaDTO>> ObtenerMovimientos(int tarjetaId);
    }
}
