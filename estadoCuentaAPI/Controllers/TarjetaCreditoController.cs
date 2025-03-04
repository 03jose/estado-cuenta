using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaCreditoController : ControllerBase
    {
        private readonly ITarjetaCreditoService _trajetaCreditoService;

        public TarjetaCreditoController(ITarjetaCreditoService trajetaCreditoService)
        {
            _trajetaCreditoService = trajetaCreditoService;
        }


        /// <summary>
        /// Agregar una tarjeta de credito a un cliente     
        /// </summary>
        /// <param name="tarjetaCreditoDTO">Informacion de la tarjeta de credito que se le va a asignar al usuario</param>
        /// <returns>
        ///     200 cuando es exitoso
        ///     400 si la tarjeta no se encuentra
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("AgregarTarjtetaCliente")]
        public async Task<string> AgregarTarjtetaCliente(TarjetaCreditoDTO tarjetaCreditoDTO)
        {
            var mensaje = await _trajetaCreditoService.CrearRegistroTarjetaCredito(tarjetaCreditoDTO);


            return mensaje;
        }

        /// <summary>
        /// Obtener una lista de tarjetas de credito de todos los clientes 
        /// </summary>
        /// <returns>
        ///     200 con la lista de tarjetas de credito
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("ObtenerListaTarjetas")]
        public async Task<IActionResult> ObtenerListaTarjetas()
        {
            try
            {
                var tarjetas = await _trajetaCreditoService.ObtenerListaTarjetas();
                return Ok(tarjetas);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocurrió un error al obtener las tarjetas.");
            }
        }
    }
}
