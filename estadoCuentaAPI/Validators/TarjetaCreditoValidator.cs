using estadoCuentaAPI.DTOs;
using FluentValidation;

namespace estadoCuentaAPI.Validators
{
    public class TarjetaCreditoValidator : AbstractValidator<TarjetaCreditoDTO>
    {
        public TarjetaCreditoValidator()
        {
            // Número de Tarjeta (Debe ser de 16 dígitos y solo números)
            RuleFor(tc => tc.NumeroTarjeta)
                .NotEmpty().WithMessage("El numero de la Tarjeta es obligatorio.")
                //.MaximumLength(16).WithMessage("El numero de la Tarjeta no debe exceder los 16 caracteres.");
                .Matches(@"^\d{16}$").WithMessage("El número de tarjeta debe tener exactamente 16 dígitos numéricos.");

            // Límite de crédito (Debe ser mayor a 0)
            RuleFor(t => t.LimiteCredito)
                .GreaterThan(0).WithMessage("El límite de crédito debe ser mayor a 0.");

            // Saldo Utilizado (No puede ser mayor al límite de crédito)
            RuleFor(t => t.SaldoUtilizado)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo utilizado no puede ser negativo.")
                .LessThanOrEqualTo(t => t.LimiteCredito)
                .WithMessage("El saldo utilizado no puede superar el límite de crédito.");

            // Tasa de interés (Debe estar entre 0% y 100%)
            RuleFor(t => t.TasaInteres)
                .InclusiveBetween(0, 100).WithMessage("La tasa de interés debe estar entre 0 y 100.");

            // Fecha de Corte (No puede ser en el pasado)
            RuleFor(t => t.FechaCorte)
                .NotEmpty().WithMessage("La fecha de corte es obligatoria")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de corte no puede ser anterior a hoy.");

            // Fecha de Pago (Debe ser después de la fecha de corte)
            RuleFor(tc => tc.FechaPago)
                .NotEmpty().WithMessage("La fecha de pago es obligatoria")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha de pago no puede ser anterior a hoy.");
        }

    }
}
