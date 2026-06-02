using Application.UseCase.Repuestos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class RepuestosController : ControllerBase
{
    private readonly IMediator _mediator;
    public RepuestosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, total) = await _mediator.Send(new GetRepuestosPaged(page, pageSize, search));
        return Ok(new { items, total, page, pageSize });
    }

    [HttpGet("bajo-stock")]
    public async Task<IActionResult> GetBajoStock()
    {
        var items = await _mediator.Send(new GetBajosStock());
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var repuesto = await _mediator.Send(new GetRepuestoById(id));
        return repuesto is null ? NotFound() : Ok(repuesto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,JefeAlmacen,JefeBodega")]
    public async Task<IActionResult> Create([FromBody] CreateRepuesto command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,JefeAlmacen,JefeBodega")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRepuesto command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:int}/stock")]
    [Authorize(Roles = "Admin,JefeAlmacen,JefeBodega,Mecanico")]
    public async Task<IActionResult> AjustarStock(int id, [FromBody] AjustarStockRequest req)
    {
        await _mediator.Send(new AjustarStock(id, req.Cantidad, req.EsIngreso));
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRepuesto(id));
        return NoContent();
    }
}

public sealed record AjustarStockRequest(int Cantidad, bool EsIngreso);
