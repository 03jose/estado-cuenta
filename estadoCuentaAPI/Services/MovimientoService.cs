using AutoMapper;
using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace estadoCuentaAPI.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MovimientoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task< List<MovimientoTarjetaDTO>> ObtenerMovimientos(int tarjetaCreditoId)
        {
            var movimientos = await _context.MovimientoTarjeta
                .FromSqlRaw("EXEC ObtenerMovimientosTarjetaPorId @p0",
                    tarjetaCreditoId)
                .Select(m => _mapper.Map<MovimientoTarjetaDTO>(m))
                .ToListAsync();

            return movimientos;
        }
    }
}
