using LiquidacaoService.API.Domain.Entities;

namespace LiquidacaoService.API.Repositories;

public interface IOperacaoRepository
{
    Task<IEnumerable<Operacao>> BuscarTodos();
    Task<IEnumerable<Operacao>> BuscarPorCliente(Guid clienteId);
    Task<Operacao?> BuscarPorId(Guid id);
    Task<IEnumerable<Operacao>> BuscarTodasLiquidadas();
    Task Adicionar(Operacao operacao);
    Task Salvar();
    Task<IEnumerable<Operacao>> BuscarPendentes();
}