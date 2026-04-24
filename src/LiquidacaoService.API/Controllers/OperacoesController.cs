using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidacaoService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperacoesController : ControllerBase
{
    private readonly IOperacaoService _service;

    public OperacoesController(IOperacaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var operacoes = await _service.BuscarTodos();
        return Ok(operacoes);
    }

    [HttpGet("pendentes")]
    public async Task<IActionResult> BuscarPendentes()
    {
        var operacoes = await _service.BuscarPendentes();
        return Ok(operacoes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var operacao = await _service.BuscarPorId(id);
        if (operacao is null) return NotFound();
        return Ok(operacao);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> BuscarPorCliente(Guid clienteId)
    {
        // O Middleware cuida do catch agora
        var operacoes = await _service.BuscarPorCliente(clienteId);
        return Ok(operacoes);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarOperacaoRequest request)
    {
        var operacao = await _service.Criar(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = operacao.Id }, operacao);
    }

    [HttpPost("seed-data")] 
    public async Task<IActionResult> GerarMassaDeTeste()
    {
        await _service.GerarMassaDeTeste();
        return Ok(new { mensagem = "Dados gerados com sucesso" });
    }

    [HttpPost("{id}/processar")]
    public async Task<IActionResult> ProcessarLiquidacao(Guid id)
    {
        var operacao = await _service.ProcessarLiquidacao(id);
        return Ok(operacao);
    }

    [HttpGet("posicao/{clienteId}")]
    public async Task<IActionResult> BuscarPosicaoCliente(Guid clienteId)
    {
        var posicao = await _service.BuscarPosicaoCliente(clienteId);
        return Ok(posicao);
    }

    [HttpGet("posicao-global")]
    public async Task<IActionResult> ObterPosicaoGlobal()
    {
        var resultado = await _service.ObterPosicaoGlobal();
        return Ok(resultado);
    }
}