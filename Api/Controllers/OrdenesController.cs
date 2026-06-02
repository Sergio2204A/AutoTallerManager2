using Application.UseCase.Ordenes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class OrdenesController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrdenesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var (items, total) = await _mediator.Send(new GetOrdenesPaged(page, pageSize, search));
        return Ok(new { items, total, page, pageSize });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orden = await _mediator.Send(new GetOrdenById(id));
        return orden is null ? NotFound() : Ok(orden);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Recepcionista")]
    public async Task<IActionResult> Create([FromBody] CreateOrden command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("{id:int}/asignar-mecanico")]
    [Authorize(Roles = "Admin,JefeTaller")]
    public async Task<IActionResult> AsignarMecanico(int id, [FromBody] AsignarMecanico command)
    {
        if (id != command.OrdenId) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/diagnostico")]
    [Authorize(Roles = "Admin,Mecanico")]
    public async Task<IActionResult> RegistrarDiagnostico(int id, [FromBody] RegistrarDiagnostico command)
    {
        if (id != command.OrdenId) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/aprobar-jefe")]
    [Authorize(Roles = "Admin,JefeTaller")]
    public async Task<IActionResult> AprobarJefe(int id)
    {
        await _mediator.Send(new AprobarOrdenJefe(id));
        return NoContent();
    }

    [HttpPost("{id:int}/aprobar-cliente")]
    [Authorize(Roles = "Admin,Recepcionista,Cliente")]
    public async Task<IActionResult> AprobarCliente(int id)
    {
        await _mediator.Send(new AprobarOrdenCliente(id));
        return NoContent();
    }

    [HttpPost("{id:int}/rechazar")]
    [Authorize(Roles = "Admin,Recepcionista,Cliente")]
    public async Task<IActionResult> Rechazar(int id, [FromBody] string? motivo)
    {
        await _mediator.Send(new RechazarOrdenCliente(id, motivo));
        return NoContent();
    }

    [HttpPost("{id:int}/iniciar")]
    [Authorize(Roles = "Admin,JefeTaller")]
    public async Task<IActionResult> Iniciar(int id)
    {
        await _mediator.Send(new IniciarReparacion(id));
        return NoContent();
    }

    [HttpPost("{id:int}/completar")]
    [Authorize(Roles = "Admin,JefeTaller")]
    public async Task<IActionResult> Completar(int id)
    {
        await _mediator.Send(new CompletarOrden(id));
        return NoContent();
    }

    [HttpPost("{id:int}/cancelar")]
    [Authorize(Roles = "Admin,JefeTaller,Recepcionista")]
    public async Task<IActionResult> Cancelar(int id, [FromBody] string? motivo)
    {
        await _mediator.Send(new CancelarOrden(id, motivo));
        return NoContent();
    }
}
