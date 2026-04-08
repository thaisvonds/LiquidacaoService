using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidacaoService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var clientes = await _service.BuscarTodos();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var cliente = await _service.BuscarPorId(id);
        if (cliente is null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteRequest request)
    {
        var cliente = await _service.Criar(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }
}