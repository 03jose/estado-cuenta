using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Models;
using estadoCuentaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly ITarjetaCreditoService _tarjetaCreditoService;
        public MovimientoController(ITarjetaCreditoService tarjetaCreditoService)
        {
            _tarjetaCreditoService = tarjetaCreditoService;
        }


        /// <summary>
        /// Registro de compras de tarjeta de credito      
        /// </summary>
        /// <param name="movimiento">objeto con las propiedades para crear un movimiento de compra</param>
        /// <returns>
        ///     200 cuando es exitoso
        ///     400 si la tarjeta no se encuentra
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("RegistrarCompra")]
        public async Task< IActionResult> RegistrarCompra([FromBody] MovimientoCompraDTO movimiento)
        {
            var resultado = await _tarjetaCreditoService.RegistrarCompraAsync(movimiento);

            
            if (!resultado.Success)
            {
                return BadRequest(new { Errors = resultado.Errors });
            }

            return Ok(new { Message = resultado.Message });
        }


        /// <summary>
        /// Registro de pago de tarjeta de credito 
        /// </summary>
        /// <param name="movimiento">objeto con las propiedades para crear un movimiento de pago</param>
        /// <returns>
        ///     200 cuando es exitoso 
        ///     400 si la tarjeta no se encuentra
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        [HttpPost("RegistrarPago")]
        public async Task< IActionResult> RegistrarPago([FromBody] PagoDTO movimiento)
        {
            var resultado = await _tarjetaCreditoService.RegistrarPagoAsync(movimiento);

            if (resultado == "Tarjeta no encontrada.")
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}
