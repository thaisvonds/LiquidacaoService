namespace LiquidacaoService.API.Services;

public interface ICalendarioService
{
    Task<DateOnly> AdicionarDiasUteis(DateOnly data, int dias);
    Task<bool> EhDiaUtil(DateOnly data);
}