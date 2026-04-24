using LiquidacaoService.API.Domain.Entities;

namespace LiquidacaoService.API.Repositories;

public interface IFeriadoRepository
{
    Task<bool> EhFeriado(DateOnly data);
    Task<IEnumerable<FeriadoCalendario>> BuscarTodos();
}