using LiquidacaoService.API.Data;
using LiquidacaoService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiquidacaoService.API.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> BuscarTodos()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> BuscarPorId(Guid id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task Adicionar(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public async Task Salvar()
    {
        await _context.SaveChangesAsync();
    }
}