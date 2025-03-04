using AutoMapper;
using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Models;
using estadoCuentaAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estadoCuentaAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<ClienteDTO> _clienteValidator;

        public ClienteService(AppDbContext context, IMapper mapper, IValidator<ClienteDTO> clienteValidator)
        {
            _context = context;
            _mapper = mapper;
            _clienteValidator = clienteValidator;
        }

        /// <summary>
        /// Crea un nuevo cliente en la base de datos 
        /// </summary>
        /// <param name="clienteDto">informacion del cliente a almacenar en la base</param>
        /// <returns>
        ///  200 si todo fue excitoso 
        ///  400 si hubo algun error de validacion
        ///  500 si hubo algun fallo en la base de datos
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<(bool Success, List<string> Errors, ClienteDTO Cliente)> CrearClienteAsync(ClienteDTO clienteDto)
        {
            // Validar el clienteDto
            var validationResult = await _clienteValidator.ValidateAsync(clienteDto);
            if (!validationResult.IsValid)
            {
                // Retornar los errores de validación
                return (false, validationResult.Errors.Select(e => e.ErrorMessage).ToList(), null);
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return (true, null, _mapper.Map<ClienteDTO>(cliente));
        }



        /// <summary>
        /// Obtener la informacion del cliente
        /// </summary>
        /// <param name="clienteId">Id del cliente a obtener la informacion</param>
        /// <returns>retorna null si no encuentra el cliente de lo contrario retorna el objeto cliente</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ClienteDTO?> ObtenerClientePorIdAsync(int clienteId)
        {
            try
            {
                var cliente =  _context.Clientes
                    .FromSqlRaw("EXEC ObtenerClientePorId @p0",
                        clienteId)
                    .AsEnumerable()
                    .FirstOrDefault();


                return cliente == null ? null : _mapper.Map<ClienteDTO>(cliente); 
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
