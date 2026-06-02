using Domain.Enums;
using MediatR;

namespace Application.UseCase.Repuestos;

public sealed record CreateRepuesto(string Codigo, string Nombre, string? Descripcion, CategoriaRepuesto Categoria, int CantidadStock, int StockMinimo, decimal PrecioCompra, decimal PrecioVenta) : IRequest<int>;
public sealed record UpdateRepuesto(int Id, string Nombre, string? Descripcion, CategoriaRepuesto Categoria, int StockMinimo, decimal PrecioCompra, decimal PrecioVenta) : IRequest;
public sealed record DeleteRepuesto(int Id) : IRequest;
public sealed record AjustarStock(int RepuestoId, int Cantidad, bool EsIngreso) : IRequest;
public sealed record GetRepuestoById(int Id) : IRequest<Domain.Entities.Taller.Repuesto?>;
public sealed record GetRepuestosPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Domain.Entities.Taller.Repuesto> Items, int Total)>;
public sealed record GetBajosStock() : IRequest<IReadOnlyList<Domain.Entities.Taller.Repuesto>>;
