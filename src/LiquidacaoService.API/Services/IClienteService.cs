using LiquidacaoService.API.DTOs;

namespace LiquidacaoService.API.Services;

public interface IClienteService
{
    Task<IEnumerable<ClienteResponse>> BuscarTodos();
    Task<ClienteResponse?> BuscarPorId(Guid id);
    Task<ClienteResponse> Criar(CriarClienteRequest request);
}