using estadoCuentaAPI.DTOs;
using FluentValidation;


namespace estadoCuentaAPI.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El apellido no debe exceder los 50 caracteres.");

            RuleFor(c => c.Nombre)
               .NotEmpty().WithMessage("El nombre es obligatorio.")
               .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.");

            RuleFor(c => c.Dui)
              .NotEmpty().WithMessage("El DUI es obligatorio.")
              .MaximumLength(10).WithMessage("El DUI no debe exceder los 10 caracteres.");


        }
    }
}
