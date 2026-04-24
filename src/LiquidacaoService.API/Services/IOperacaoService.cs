using LiquidacaoService.API.DTOs;

namespace LiquidacaoService.API.Services;

public interface IOperacaoService
{
    Task<IEnumerable<OperacaoResponse>> BuscarTodos();
    Task<IEnumerable<OperacaoResponse>> BuscarPorCliente(Guid clienteId);
    Task<OperacaoResponse?> BuscarPorId(Guid id);
    Task<OperacaoResponse> Criar(CriarOperacaoRequest request);
    Task GerarMassaDeTeste();
    Task<OperacaoResponse> ProcessarLiquidacao(Guid operacaoId);
    Task<IEnumerable<OperacaoResponse>> BuscarPendentes();
    Task<PosicaoClienteResponse> BuscarPosicaoCliente(Guid clienteId);
    Task<IEnumerable<PosicaoAtivoGlobalResponse>> ObterPosicaoGlobal();
}