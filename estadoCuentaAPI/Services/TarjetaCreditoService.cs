using AutoMapper;
using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.Models;
using estadoCuentaAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace estadoCuentaAPI.Services
{
    // Cálculo del interés bonificable, cuota mínima y monto total a pagar

    public class TarjetaCreditoService : ITarjetaCreditoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<TarjetaCreditoDTO> _tarjetaCreditoValidator;

        // Inyección de dependencias del contexto
        public TarjetaCreditoService(AppDbContext context, IMapper mapper, IValidator<TarjetaCreditoDTO> tarjetaCreditoValidator)
        {
            _context = context;
            _mapper = mapper;
            _tarjetaCreditoValidator = tarjetaCreditoValidator;
        }

        /// <summary>
        /// Obtener estado de cuenta del cliente por medio de la tarjeta
        /// </summary>
        /// <param name="tarjetaCreditoId">ID tarjeta de credito</param>
        /// <param name="porcentajeInteres">porcentaje de interes</param>
        /// <param name="porcentajeMinimo">porcentaje para el pago minimo</param>
        /// <returns>retorna un objeto con la informacion del estado de cuenta</returns>
        /// <exception cref="Exception"></exception>
        public async Task< EstadoCuentaDTO> ObtenerEstadoCuentaAsync(int tarjetaCreditoId)
        {
            var tarjeta =
                _context.TarjetaCreditos
                .FromSqlRaw("EXEC ObtenerInformacionTarjeta @p0",
                    tarjetaCreditoId)  
                .AsEnumerable()
                .FirstOrDefault() ?? throw new Exception("Tarjeta no encontrada.");
            var movimientos = await ObtenerHistorialTransaccionesAsync(tarjetaCreditoId);
                //_context.MovimientoTarjeta.Where(m => m.TarjetaCreditoId == tarjetaCreditoId).ToList();
            var saldoTotal = tarjeta.SaldoUtilizado;
            var montoComprasMesActual = movimientos.Where(m => m.FechaMoviomiento.Month == DateTime.Now.Month).Sum(m => m.Monto);
            var montoComprasMesAnterior = movimientos.Where(m => m.FechaMoviomiento.Month == DateTime.Now.AddMonths(-1).Month).Sum(m => m.Monto);

            // Calcular interés bonificable
            var interesBonificable = saldoTotal * tarjeta.TasaInteresConfigurable / 100;

            // Calcular cuota mínima
            var cuotaMinima = saldoTotal * tarjeta.PorcentajeSaldoMin / 100;

            // Calcular monto total a pagar
            var montoTotalPagar = saldoTotal + montoComprasMesActual;

            // Calcular monto total de contado con intereses
            var montoContadoConIntereses = saldoTotal + interesBonificable;

            var nombreCompletoCliente = "";

            if (tarjeta.ClienteId.HasValue)
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == tarjeta.ClienteId);
                nombreCompletoCliente = cliente != null ?  $"{cliente.Nombre} {cliente.Apellido}" : "";
            }

            return new EstadoCuentaDTO
            {
                NombreTitular = nombreCompletoCliente,
                NumeroTarjeta = tarjeta.NumeroTarjeta,
                
                LimiteCredito = tarjeta.LimiteCredito,
                SaldoDisponible = tarjeta.LimiteCredito - saldoTotal,
                SaldoUtilizado = saldoTotal,
                Movimientos = movimientos.Where(m=> m.TipoMovimiento == "compra").Select(m => new MovimientoDTO
                {
                    FechaMoviomiento = m.FechaMoviomiento,
                    Descripcion = m.Descripcion,
                    Monto = m.Monto
                }).ToList(),
                MontoTotalComprasMesActual = montoComprasMesActual,
                MontoTotalComprasMesAnterior = montoComprasMesAnterior,
                InteresBonificable = interesBonificable,
                CuotaMinima = cuotaMinima,
                MontoTotalPagar = montoTotalPagar,
                MontoContadoConIntereses = montoContadoConIntereses
            };
        }


        /// <summary>
        /// Obtener Historial de Transacciones de la tarjeta
        /// </summary>
        /// <param name="tarjetaCreditoId">ID de la tarjeta</param>
        /// <returns>una lista de movimientos de la tarjeta</returns>
        public async Task<List<MovimientoDTO>> ObtenerHistorialTransaccionesAsync(int tarjetaCreditoId)
        {
            try
            {
                var movimientos = _context.MovimientoTarjeta
                    .FromSqlRaw("EXEC ObtenerMovimientosTarjetaPorId @p0",
                        tarjetaCreditoId)
                    .AsEnumerable()
                    .Select(m => _mapper.Map<MovimientoDTO>(m))
                    .ToList();


                return movimientos;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Registrar tarjeta de credito a usuario 
        /// </summary>
        /// <returns>mensaje de creacion de tarjeta exitoso o de error en caso de fallo</returns>
        public async Task<string> CrearRegistroTarjetaCredito(TarjetaCreditoDTO tarjetaCreditoDTO)
        {
            if (!tarjetaCreditoDTO.ClienteId.HasValue)
                return "El cliente no es valido";

            var cliente = _context.Clientes
             .FromSqlRaw("EXEC ObtenerClientePorId @p0",
                 tarjetaCreditoDTO.ClienteId)
             .AsEnumerable()
             .FirstOrDefault();

            if (cliente == null)
                return "Cliente no encontrado";

            TarjetaCredito tarjetaCredito = _mapper.Map<TarjetaCredito>(tarjetaCreditoDTO);
            _context.TarjetaCreditos.Add(tarjetaCredito);
            await _context.SaveChangesAsync();

            return "tarjeta creada exitosamente";
        }

        /// <summary>
        /// Obtencion de los movimientos en un rango de fechas seleccionado
        /// </summary>
        /// <param name="tarjetaCreditoId">Id de la tarjeta de credito</param>
        /// <param name="fechaInicio">fecha de inicio de la que se obtendra la informacion de los movimientos</param>
        /// <param name="fechaFin">fecha fin de la que se obtendra la informacion</param>
        /// <returns>
        ///    movimientos de la tarjeta en el rango de fechas dado
        ///    500 si hubo algun error 
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<MovimientoDTO>> ObtenerComprasEnRangoAsync(int tarjetaCreditoId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                 
                var movimientos = _context.MovimientoTarjeta
                    .FromSqlRaw("EXEC ObtenerMovimientosTarjeta @p0, @p1, @p2",
                        tarjetaCreditoId, fechaInicio, fechaFin)
                    .AsEnumerable()
                    .Select(m => _mapper.Map<MovimientoDTO>(m))
                    .ToList();

                return movimientos;
            }
            catch(Exception e)
            {
                return new List<MovimientoDTO>();
            }
        }


        /// <summary>
        /// Registro la compra en la tabla de movimientos
        /// </summary>
        /// <param name="movimiento">recibe el objeto con la infomracion de la compra</param>
        /// <returns>devuelve el mensaje de guardado o de error </returns>
        public async Task<(bool Success, List<string> Errors, string Message)> RegistrarCompraAsync(MovimientoCompraDTO movimiento)
        {
            var errores = new List<string>();

            // Buscar la tarjeta en la base de datos
            var tarjeta = _context.TarjetaCreditos
                .FromSqlRaw("EXEC ObtenerInformacionTarjeta @p0", movimiento.TarjetaCreditoId)
                .AsEnumerable()
                .FirstOrDefault();

            var movimientoTarjeta = _mapper.Map<MovimientoTarjetaDTO>(movimiento);

            if (tarjeta == null)
            {
                errores.Add("Tarjeta no encontrada.");
            }
            else
            {
                // Validar la tarjeta con FluentValidation
                var movimientoValidator = new MovimientoTarjetaValidator();                
                movimientoTarjeta.TipoMovimiento = "Compra";
                var resultadoValidacionTarjeta = await movimientoValidator.ValidateAsync(movimientoTarjeta);
                if (!resultadoValidacionTarjeta.IsValid)
                {
                    errores.AddRange(resultadoValidacionTarjeta.Errors.Select(e => e.ErrorMessage));
                }

                // Validar si la tarjeta tiene saldo suficiente
                if (tarjeta.SaldoUtilizado + movimiento.Monto > tarjeta.LimiteCredito)
                {
                    errores.Add("El monto excede el límite de crédito disponible.");
                }
            }

            // Si hay errores, retornar la lista
            if (errores.Any())
            {
                return (false, errores, "Error en la validación.");
            }

            // Crear el nuevo movimiento
            //var nuevoMovimiento = _mapper.Map<MovimientoTarjetum>(movimientoTarjeta);
            var nuevoMovimiento = new MovimientoTarjetum
            {
                TarjetaCreditoId = movimientoTarjeta.TarjetaCreditoId,
                FechaMovimiento = movimientoTarjeta.FechaMovimiento,
                Descripcion = movimientoTarjeta.Descripcion,
                Monto = movimientoTarjeta.Monto,
                TipoMovimiento = movimientoTarjeta.TipoMovimiento
            };

            _context.MovimientoTarjeta.Add(nuevoMovimiento);

            // Actualizar el saldo de la tarjeta
            tarjeta.SaldoUtilizado += movimiento.Monto;

            await _context.SaveChangesAsync();

            return (true, null, "Movimiento registrado exitosamente.");
        }


        public async Task<string> RegistrarPagoAsync(PagoDTO movimiento)
        {
            // Instanciar el validador
            var validator = new MovimientoTarjetaValidator();
            var validationResult = validator.Validate(new MovimientoTarjetaDTO
            {
                TarjetaCreditoId = movimiento.TarjetaCreditoId,
                FechaMovimiento = movimiento.FechaPago,
                Descripcion = movimiento.Descripcion,
                Monto = movimiento.Monto,
                TipoMovimiento = "Pago"
            });

            // Si la validación falla, devolver los errores
            if (!validationResult.IsValid)
            {
                return string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Buscar la tarjeta asociada al movimiento
            var tarjeta = _context.TarjetaCreditos
                .FromSqlRaw("EXEC ObtenerInformacionTarjeta @p0",
                    movimiento.TarjetaCreditoId)
                .AsEnumerable()
                .FirstOrDefault();

            if (tarjeta == null)
                return "Tarjeta no encontrada.";

            // Crear el nuevo movimiento
            var nuevoMovimiento = new MovimientoTarjetum
            {
                TarjetaCreditoId = movimiento.TarjetaCreditoId,
                FechaMovimiento = movimiento.FechaPago,
                Descripcion = movimiento.Descripcion,
                Monto = movimiento.Monto,
                TipoMovimiento = "Pago"
            };

            // Agregar el nuevo movimiento a la base de datos
            _context.MovimientoTarjeta.Add(nuevoMovimiento);

            // Actualizar el saldo de la tarjeta
            tarjeta.SaldoUtilizado += movimiento.Monto;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return "Movimiento registrado exitosamente.";
        }

        public async Task<List<ClienteTarjetaDTO>> ObtenerListaTarjetas()
        {
            try
            {
                var tarjetas =  _context.ClienteTarjetaViews
                    .FromSqlRaw("EXEC ObtenerListadoTarjetas")
                    .AsEnumerable()
                    .Select(t => _mapper.Map<ClienteTarjetaDTO>(t))
                    .ToList();

                return tarjetas;
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
