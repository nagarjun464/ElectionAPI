using Microsoft.AspNetCore.Mvc;
using Election.API.Data;
using Election.API.Dtos;
using Election.API.Mapping;

namespace Election.API.Controllers;

[ApiController]                                   
[Route("api/[controller]")]                       
public class ElectionsController : ControllerBase
{
    private readonly IElectionRepository _repo;

    public ElectionsController(IElectionRepository repo) => _repo = repo;

    [HttpPost]
    [ProducesResponseType(typeof(ElectionDto), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    public async Task<IActionResult> Create([FromBody] CreateElectionDto dto, CancellationToken ct)
    {

        if (dto.EndUtc <= dto.StartUtc)
            ModelState.AddModelError(nameof(dto.EndUtc), "EndUtc must be after StartUtc.");
        if (!ModelState.IsValid) return ValidationProblem();

        var model = ElectionMapper.ToModel(dto);
        var id = await _repo.CreateAsync(model, ct);
        var saved = await _repo.GetAsync(id, ct);
        return CreatedAtAction(nameof(Get), new { id }, ElectionMapper.ToDto(saved!));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ElectionDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var e = await _repo.GetAsync(id, ct);
        return e is null ? NotFound() : Ok(ElectionMapper.ToDto(e));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ElectionDto>), 200)]
    public async Task<IActionResult> List([FromQuery] int limit = 50, [FromQuery] string? status = null, CancellationToken ct = default)
    {
        limit = Math.Clamp(limit, 1, 200);
        var items = await _repo.ListAsync(limit, status, ct);
        return Ok(items.Select(ElectionMapper.ToDto));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateElectionDto dto, CancellationToken ct)
    {
        if (dto.EndUtc <= dto.StartUtc)
            ModelState.AddModelError(nameof(dto.EndUtc), "EndUtc must be after StartUtc.");
        if (!ModelState.IsValid) return ValidationProblem();

        var existing = await _repo.GetAsync(id, ct);
        if (existing is null) return NotFound();

        ElectionMapper.Apply(dto, existing);
        var ok = await _repo.UpdateAsync(existing, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var ok = await _repo.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
