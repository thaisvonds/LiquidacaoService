using LiquidacaoService.API.Repositories;

namespace LiquidacaoService.API.Services;

public class CalendarioService : ICalendarioService
{
    private readonly IFeriadoRepository _feriadoRepository;

    public CalendarioService(IFeriadoRepository feriadoRepository)
    {
        _feriadoRepository = feriadoRepository;
    }

    public async Task<bool> EhDiaUtil(DateOnly data)
    {
        // Fim de semana
        if (data.DayOfWeek == DayOfWeek.Saturday || 
            data.DayOfWeek == DayOfWeek.Sunday)
            return false;

        // Feriado
        var ehFeriado = await _feriadoRepository.EhFeriado(data);
        return !ehFeriado;
    }

    public async Task<DateOnly> AdicionarDiasUteis(DateOnly data, int dias)
    {
        var dataAtual = data;
        var diasAdicionados = 0;

        while (diasAdicionados < dias)
        {
            dataAtual = dataAtual.AddDays(1);
            if (await EhDiaUtil(dataAtual))
                diasAdicionados++;
        }

        return dataAtual;
    }
}