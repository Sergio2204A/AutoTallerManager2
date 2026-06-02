using Application.UseCase.Auditorias;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class AuditoriasController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuditoriasController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
    {
        var (items, total) = await _mediator.Send(new GetAuditoriasPaged(page, pageSize, search));
        return Ok(new { items, total, page, pageSize });
    }
}
