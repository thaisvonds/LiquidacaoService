using LiquidacaoService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiquidacaoService.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Liquidacao> Liquidacoes { get; set; }
    public DbSet<FeriadoCalendario> FeriadosCalendario { get; set; }
}