using LiquidacaoService.API.Data;
using LiquidacaoService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiquidacaoService.API.Repositories;

public class AtivoRepository : IAtivoRepository
{
    private readonly AppDbContext _context;

    public AtivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ativo>> BuscarTodos()
    {
        return await _context.Ativos.ToListAsync();
    }

    public async Task<Ativo?> BuscarPorId(Guid id)
    {
        return await _context.Ativos.FindAsync(id);
    }

    public async Task<Ativo?> BuscarPorCodigo(string codigo)
    {
        return await _context.Ativos
            .FirstOrDefaultAsync(a => a.Codigo == codigo.ToUpper());
    }

    public async Task Adicionar(Ativo ativo)
    {
        await _context.Ativos.AddAsync(ativo);
    }

    public async Task Salvar()
    {
        await _context.SaveChangesAsync();
    }
}