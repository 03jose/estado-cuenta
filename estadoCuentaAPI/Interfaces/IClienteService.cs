using estadoCuentaAPI.DTOs;

namespace estadoCuentaAPI.Interfaces
{
    public interface IClienteService
    {
        //Task<ClienteDTO> CrearClienteAsync(ClienteDTO clienteDto);
        Task<(bool Success, List<string>? Errors, ClienteDTO Cliente)> CrearClienteAsync(ClienteDTO clienteDto);
        Task<ClienteDTO> ObtenerClientePorIdAsync(int clienteId);
    }
}
