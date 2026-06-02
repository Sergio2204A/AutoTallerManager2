using FluentValidation;

namespace Application.UseCase.Repuestos;

public sealed class CreateRepuestoValidator : AbstractValidator<CreateRepuesto>
{
    public CreateRepuestoValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CantidadStock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockMinimo).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PrecioCompra).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PrecioVenta).GreaterThanOrEqualTo(0);
    }
}

public sealed class UpdateRepuestoValidator : AbstractValidator<UpdateRepuesto>
{
    public UpdateRepuestoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.StockMinimo).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PrecioCompra).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PrecioVenta).GreaterThanOrEqualTo(0);
    }
}
