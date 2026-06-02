using FluentValidation;

namespace Application.UseCase.Vehiculos;

public sealed class CreateVehiculoValidator : AbstractValidator<CreateVehiculo>
{
    public CreateVehiculoValidator()
    {
        RuleFor(x => x.Marca).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Anio).InclusiveBetween(1900, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.Vin).NotEmpty().MaximumLength(17);
        RuleFor(x => x.Kilometraje).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Placa).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Color).NotEmpty().MaximumLength(30);
        RuleFor(x => x.ClienteId).GreaterThan(0);
    }
}
