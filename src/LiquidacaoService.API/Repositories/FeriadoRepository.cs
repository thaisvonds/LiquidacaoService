using LiquidacaoService.API.Data;
using LiquidacaoService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiquidacaoService.API.Repositories;

public class FeriadoRepository : IFeriadoRepository
{
    private readonly AppDbContext _context;

    public FeriadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EhFeriado(DateOnly data)
    {
        return await _context.FeriadosCalendario
            .AnyAsync(f => f.Data == data);
    }

    public async Task<IEnumerable<FeriadoCalendario>> BuscarTodos()
    {
        return await _context.FeriadosCalendario
            .OrderBy(f => f.Data)
            .ToListAsync();
    }
}