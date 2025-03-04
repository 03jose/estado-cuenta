using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// crear un nuevo cliente en la base de datos
        /// </summary>
        /// <param name="clienteDto">objeto con las propiedades para crear al cliente</param>
        /// <returns>codigo de exito o de error</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteDTO clienteDto)
        {
            var result = await _clienteService.CrearClienteAsync(clienteDto);

            if (!result.Success)
            {
                return BadRequest(new { Errors = result.Errors });
            }

            return Ok(result.Cliente);
        }

        /// <summary>
        /// Obtener un cliente por su id
        /// </summary>
        /// <param name="clienteId">Id del cliente </param>
        /// <returns>
        ///     200 con la informacion del cliente 
        ///     404 si no se encuentra el cliente
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{clienteId}")]
        public async Task<IActionResult> ObtenerClientePorId(int clienteId)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(clienteId);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }
    }
}
