using Application.UseCase.Facturas;
using Application.UseCase.Pagos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class FacturasController : ControllerBase
{
    private readonly IMediator _mediator;
    public FacturasController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, total) = await _mediator.Send(new GetFacturasPaged(page, pageSize, search));
        return Ok(new { items, total, page, pageSize });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var factura = await _mediator.Send(new GetFacturaById(id));
        return factura is null ? NotFound() : Ok(factura);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Recepcionista")]
    public async Task<IActionResult> Generar([FromBody] GenerarFactura command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:int}/pagos")]
    public async Task<IActionResult> GetPagos(int id)
    {
        var pagos = await _mediator.Send(new GetPagosByFactura(id));
        return Ok(pagos);
    }

    [HttpPost("{id:int}/pagos")]
    [Authorize(Roles = "Admin,Recepcionista")]
    public async Task<IActionResult> RegistrarPago(int id, [FromBody] RegistrarPago command)
    {
        if (id != command.FacturaId) return BadRequest();
        var pagoId = await _mediator.Send(command);
        return Ok(new { pagoId });
    }
}
