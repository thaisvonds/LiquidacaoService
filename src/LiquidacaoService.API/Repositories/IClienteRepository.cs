using LiquidacaoService.API.Domain.Entities;

namespace LiquidacaoService.API.Repositories;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> BuscarTodos();
    Task<Cliente?> BuscarPorId(Guid id);
    Task Adicionar(Cliente cliente);
    Task Salvar();
}