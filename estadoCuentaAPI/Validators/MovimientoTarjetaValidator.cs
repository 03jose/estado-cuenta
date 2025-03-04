using estadoCuentaAPI.DTOs;
using FluentValidation;

namespace estadoCuentaAPI.Validators
{
    public class MovimientoTarjetaValidator : AbstractValidator<MovimientoTarjetaDTO>
    {
        public MovimientoTarjetaValidator()
        {
            // TarjetaCreditoId (Opcional pero si está presente debe ser mayor a 0)
            RuleFor(m => m.TarjetaCreditoId)
                .GreaterThan(0).WithMessage("El ID de la tarjeta de crédito debe ser un número positivo.");

            // FechaMovimiento No puede ser en el futuro
            RuleFor(m => m.FechaMovimiento)
                 .Must(fecha => fecha.Date <= DateTime.Today)
                .WithMessage("La fecha del movimiento no puede ser futura.");

            // Descripción Obligatoria, mínimo 3 caracteres
            RuleFor(m => m.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MinimumLength(3).WithMessage("La descripción debe tener al menos 3 caracteres.");

            // Monto Debe ser mayor a 0
            RuleFor(m => m.Monto)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

            // TipoMovimiento Debe ser 'Compra' o 'Pago'
            RuleFor(m => m.TipoMovimiento)
                .NotEmpty().WithMessage("El tipo de movimiento es obligatorio.")
                .Must(tipo => tipo == "Compra" || tipo == "Pago")
                .WithMessage("El tipo de movimiento debe ser 'Compra' o 'Pago'.");
        }
    }
}
