using FluentValidation;

namespace Application.UseCase.Clientes;

public sealed class UpdateClienteValidator : AbstractValidator<UpdateCliente>
{
    public UpdateClienteValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.NombreCompleto).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
    }
}
