using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Application.Contracts.Tickets;
using OfflineTicketing.Application.Services;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Api.Controllers;

[ApiController]
[Route("tickets")]
public sealed class TicketsController : ControllerBase
{
    private readonly ICurrentUser _current;
    private readonly TicketService _tickets;
    private readonly TicketStatsService _stats;

    public TicketsController(ICurrentUser current, TicketService tickets, TicketStatsService stats)
    {
        _current = current;
        _tickets = tickets;
        _stats = stats;
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request, CancellationToken ct)
    {
        var userId = _current.UserId ?? throw new InvalidOperationException("UserId is missing.");
        var created = await _tickets.CreateAsync(userId, request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Employee")]
    [ProducesResponseType(typeof(List<TicketResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> My(CancellationToken ct)
    {
        var userId = _current.UserId ?? throw new InvalidOperationException("UserId is missing.");
        return Ok(await _tickets.GetMyAsync(userId, ct));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<TicketResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> All(CancellationToken ct)
        => Ok(await _tickets.GetAllAsync(ct));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketRequest request, CancellationToken ct)
    {
        try
        {
            var updated = await _tickets.UpdateByAdminAsync(id, request, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TicketStatsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Stats(CancellationToken ct)
        => Ok(await _stats.GetAsync(ct));

    // Bonus: creator (employee) OR assigned admin
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var userId = _current.UserId;
        var role = _current.Role;

        if (userId is null || role is null)
            return Forbid();

        var ticket = await _tickets.GetByIdForUserAsync(id, userId.Value, role.Value, ct);
        if (ticket is null)
        {
            // differentiate "not found" vs "forbidden" is expensive without another query; keep simple.
            return Forbid();
        }

        return Ok(ticket);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _tickets.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
