using FluentValidation;

namespace Application.UseCase.Vehiculos;

public sealed class UpdateVehiculoValidator : AbstractValidator<UpdateVehiculo>
{
    public UpdateVehiculoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Marca).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Anio).InclusiveBetween(1900, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.Kilometraje).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Color).NotEmpty().MaximumLength(30);
    }
}
