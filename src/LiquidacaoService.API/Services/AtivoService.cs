using LiquidacaoService.API.Domain.Entities;
using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Domain.Enums;
using LiquidacaoService.API.Repositories;

namespace LiquidacaoService.API.Services;

public class AtivoService : IAtivoService
{
    private readonly IAtivoRepository _repository;

    public AtivoService(IAtivoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AtivoResponse>> BuscarTodos()
    {
        var ativos = await _repository.BuscarTodos();
        return ativos.Select(a => ParaResponse(a));
    }

    public async Task<AtivoResponse?> BuscarPorId(Guid id)
    {
        var ativo = await _repository.BuscarPorId(id);
        if (ativo is null) return null;
        return ParaResponse(ativo);
    }

    public async Task<AtivoResponse?> BuscarPorCodigo(string codigo)
    {
        var ativo = await _repository.BuscarPorCodigo(codigo);
        if (ativo is null) return null;
        return ParaResponse(ativo);
    }

    public async Task<AtivoResponse> Criar(CriarAtivoRequest request)
    {
        var ativo = new Ativo
        {
            Id = Guid.NewGuid(),
            Codigo = request.Codigo.ToUpper(),
            Descricao = request.Descricao,
            Tipo = request.Tipo,
            PrazoLiquidacao = request.PrazoLiquidacao
        };

        await _repository.Adicionar(ativo);
        await _repository.Salvar();

        return ParaResponse(ativo);
    }

    private AtivoResponse ParaResponse(Ativo ativo)
    {
        return new AtivoResponse
        {
            Id = ativo.Id,
            Codigo = ativo.Codigo,
            Descricao = ativo.Descricao,
            Tipo = ativo.Tipo,
            PrazoLiquidacao = ativo.PrazoLiquidacao
        };
    }
}