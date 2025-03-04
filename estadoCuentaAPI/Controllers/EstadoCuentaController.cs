using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoCuentaController : ControllerBase
    {
        private readonly ITarjetaCreditoService _estadoCuentaService;

        public EstadoCuentaController(ITarjetaCreditoService estadoCuentaService)
        {
            _estadoCuentaService = estadoCuentaService;
        }

        /// <summary>
        /// Obtener informacion del estado de cuenta de una tarjeta de credito especifica
        /// </summary>
        /// <param name="tarjetaId">Id de la tarjeta de credito</param>
        /// <returns>
        ///     200 con la informacion del estado de cuenta
        ///     404 si no se encuentra el estado de cuenta
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("ObtenerEstadoCuenta/{tarjetaId}")]
        public async Task<IActionResult?> ObtenerEstadoCuenta(int tarjetaId)
        {
            if (tarjetaId == 0) return null;
            var EstadoCuenta =  await _estadoCuentaService.ObtenerEstadoCuentaAsync(tarjetaId);
            return Ok(EstadoCuenta);
        }
    }
}
