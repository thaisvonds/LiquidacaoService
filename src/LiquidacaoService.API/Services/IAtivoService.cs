using LiquidacaoService.API.DTOs;

namespace LiquidacaoService.API.Services;

public interface IAtivoService
{
    Task<IEnumerable<AtivoResponse>> BuscarTodos();
    Task<AtivoResponse?> BuscarPorId(Guid id);
    Task<AtivoResponse?> BuscarPorCodigo(string codigo);
    Task<AtivoResponse> Criar(CriarAtivoRequest request);
}