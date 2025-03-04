using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialTransaccionesController : ControllerBase
    {
        private readonly ITarjetaCreditoService _tarjetaCreditoService;

        public HistorialTransaccionesController(ITarjetaCreditoService tarjetaCreditoService)
        {
            _tarjetaCreditoService = tarjetaCreditoService;
        }

        /// <summary>
        /// obtener historial de transacciones de la tarjeta de credito especificada por ID  
        /// </summary>
        /// <param name="tarjetaCreditoId">ID de la tarjeta de credito</param>
        /// <returns>
        ///     200 con el historial de transacciones o un error 
        ///     500 si ocurre algun fallo
        /// </returns>
        [HttpGet("historialTransacciones/{tarjetaCreditoId}")]
        
        public async Task<ActionResult<List<MovimientoDTO>>> ObtenerHistorialTransacciones(int tarjetaCreditoId)
        {
            try
            {
                var historial = await _tarjetaCreditoService.ObtenerHistorialTransaccionesAsync(tarjetaCreditoId);
                if (historial == null || !historial.Any())
                {
                    return NotFound("No se encontraron transacciones para esta tarjeta.");
                }
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
