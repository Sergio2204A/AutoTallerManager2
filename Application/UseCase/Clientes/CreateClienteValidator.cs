using FluentValidation;

namespace Application.UseCase.Clientes;

public sealed class CreateClienteValidator : AbstractValidator<CreateCliente>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.NombreCompleto).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Cedula).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
