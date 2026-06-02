using Application.UseCase.Vehiculos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class VehiculosController : ControllerBase
{
    private readonly IMediator _mediator;
    public VehiculosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, total) = await _mediator.Send(new GetVehiculosPaged(page, pageSize, search));
        return Ok(new { items, total, page, pageSize });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehiculo = await _mediator.Send(new GetVehiculoById(id));
        return vehiculo is null ? NotFound() : Ok(vehiculo);
    }

    [HttpGet("cliente/{clienteId:int}")]
    public async Task<IActionResult> GetByCliente(int clienteId)
    {
        var vehiculos = await _mediator.Send(new GetVehiculosByCliente(clienteId));
        return Ok(vehiculos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehiculo command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehiculo command)
    {
        if (id != command.Id) return BadRequest("El Id no coincide.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Recepcionista")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteVehiculo(id));
        return NoContent();
    }
}
