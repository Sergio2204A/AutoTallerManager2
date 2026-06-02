using FluentValidation;

namespace Application.UseCase.Ordenes;

public sealed class CreateOrdenValidator : AbstractValidator<CreateOrden>
{
    public CreateOrdenValidator()
    {
        RuleFor(x => x.VehiculoId).GreaterThan(0);
        RuleFor(x => x.ClienteId).GreaterThan(0);
    }
}

public sealed class RegistrarDiagnosticoValidator : AbstractValidator<RegistrarDiagnostico>
{
    public RegistrarDiagnosticoValidator()
    {
        RuleFor(x => x.OrdenId).GreaterThan(0);
        RuleFor(x => x.Diagnostico).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.PropuestaReparacion).NotEmpty().MaximumLength(2000);
    }
}
