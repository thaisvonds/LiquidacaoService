using LiquidacaoService.API.Domain.Entities;

namespace LiquidacaoService.API.Repositories;

public interface IAtivoRepository
{
    Task<IEnumerable<Ativo>> BuscarTodos();
    Task<Ativo?> BuscarPorId(Guid id);
    Task<Ativo?> BuscarPorCodigo(string codigo);
    Task Adicionar(Ativo ativo);
    Task Salvar();
}