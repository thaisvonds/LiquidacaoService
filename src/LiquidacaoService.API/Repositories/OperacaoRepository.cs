using LiquidacaoService.API.Data;
using LiquidacaoService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.API.Repositories;

public class OperacaoRepository : IOperacaoRepository
{
    private readonly AppDbContext _context;

    public OperacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operacao>> BuscarTodos()
    {
        return await _context.Operacoes
            .Include(o => o.Cliente)
            .Include(o => o.Ativo)
            .Include(o => o.Liquidacao)
            .OrderByDescending(o => o.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<Operacao>> BuscarPorCliente(Guid clienteId)
    {
        return await _context.Operacoes
            .Include(o => o.Cliente)
            .Include(o => o.Ativo)
            .Include(o => o.Liquidacao)
            .Where(o => o.ClienteId == clienteId)
            .OrderByDescending(o => o.CriadoEm)
            .ToListAsync();
    }

    public async Task<Operacao?> BuscarPorId(Guid id)
    {
        return await _context.Operacoes
            .Include(o => o.Cliente)
            .Include(o => o.Ativo)
            .Include(o => o.Liquidacao)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Operacao>> BuscarTodasLiquidadas()
    {
        return await _context.Operacoes
            .Include(o => o.Ativo)
            .Where(o => o.Status == StatusOperacao.Liquidada)
            .ToListAsync();
    }

    public async Task Adicionar(Operacao operacao)
    {
        await _context.Operacoes.AddAsync(operacao);
    }

    public async Task Salvar()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Operacao>> BuscarPendentes()
    {
        return await _context.Operacoes
            .Include(o => o.Cliente)
            .Include(o => o.Ativo)
            .Include(o => o.Liquidacao)
            .Where(o => o.Status == StatusOperacao.Pendente && o.Liquidacao != null)
            .OrderBy(o => o.Liquidacao!.DataPrevista)
            .ToListAsync();
    }
}