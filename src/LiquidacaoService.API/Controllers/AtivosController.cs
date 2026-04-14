using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidacaoService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtivosController : ControllerBase
{
    private readonly IAtivoService _service;

    public AtivosController(IAtivoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var ativos = await _service.BuscarTodos();
        return Ok(ativos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var ativo = await _service.BuscarPorId(id);
        if (ativo is null) return NotFound();
        return Ok(ativo);
    }

    [HttpGet("codigo/{codigo}")]
    public async Task<IActionResult> BuscarPorCodigo(string codigo)
    {
        var ativo = await _service.BuscarPorCodigo(codigo);
        if (ativo is null) return NotFound();
        return Ok(ativo);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAtivoRequest request)
    {
        var ativo = await _service.Criar(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = ativo.Id }, ativo);
    }
}