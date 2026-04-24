using LiquidacaoService.API.Domain.Entities;
using LiquidacaoService.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LiquidacaoService.API.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedAtivos(context);
        await SeedClientes(context);
        await SeedFeriados(context);
        await context.SaveChangesAsync();
    }

   private static async Task SeedAtivos(AppDbContext context)
{
    var codigosDesejados = new List<string> { "PETR4", "VALE3", "ITUB4", "BBAS3", "MXRF11", "HGLG11", "TESOURO-SELIC" };
    
    // Pega o que já existe no banco para não duplicar
    var ativosExistentes = await context.Ativos
        .Where(a => codigosDesejados.Contains(a.Codigo))
        .Select(a => a.Codigo)
        .ToListAsync();

    var novosAtivos = new List<Ativo>();

    // Só adiciona se o código não estiver no banco
    if (!ativosExistentes.Contains("PETR4"))
        novosAtivos.Add(new() { Id = Guid.NewGuid(), Codigo = "PETR4", Descricao = "Petrobras PN", Tipo = TipoAtivo.RendaVariavel, PrazoLiquidacao = 2 });
    
    if (!ativosExistentes.Contains("VALE3"))
        novosAtivos.Add(new() { Id = Guid.NewGuid(), Codigo = "VALE3", Descricao = "Vale S.A. ON", Tipo = TipoAtivo.RendaVariavel, PrazoLiquidacao = 2 });
    
    if (!ativosExistentes.Contains("ITUB4"))
        novosAtivos.Add(new() { Id = Guid.NewGuid(), Codigo = "ITUB4", Descricao = "Itaú Unibanco PN", Tipo = TipoAtivo.RendaVariavel, PrazoLiquidacao = 2 });


    if (novosAtivos.Any())
    {
        await context.Ativos.AddRangeAsync(novosAtivos);
    }
}

    private static async Task SeedClientes(AppDbContext context)
    {
        if (await context.Clientes.AnyAsync()) return;

        var clientes = new List<Cliente>
        {
            new() { Id = Guid.NewGuid(), Nome = "Ana Silva", Cpf = "11122233344", Email = "ana.silva@email.com" },
            new() { Id = Guid.NewGuid(), Nome = "Carlos Mendes", Cpf = "22233344455", Email = "carlos.mendes@email.com" },
            new() { Id = Guid.NewGuid(), Nome = "Thais Cavalcante", Cpf = "33344455566", Email = "thais@email.com" },
        };

        await context.Clientes.AddRangeAsync(clientes);
    }

    private static async Task SeedFeriados(AppDbContext context)
    {
        if (await context.FeriadosCalendario.AnyAsync()) return;

        var feriados = new List<FeriadoCalendario>
        {
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 1, 1), Nome = "Confraternização Universal" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 2, 16), Nome = "Carnaval" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 2, 17), Nome = "Carnaval" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 4, 3), Nome = "Sexta-feira Santa" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 4, 21), Nome = "Tiradentes" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 5, 1), Nome = "Dia do Trabalho" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 6, 4), Nome = "Corpus Christi" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 9, 7), Nome = "Independência do Brasil" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 10, 12), Nome = "Nossa Senhora Aparecida" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 11, 2), Nome = "Finados" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 11, 15), Nome = "Proclamação da República" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 11, 20), Nome = "Consciência Negra" },
            new() { Id = Guid.NewGuid(), Data = new DateOnly(2026, 12, 25), Nome = "Natal" },
        };

        await context.FeriadosCalendario.AddRangeAsync(feriados);
    }
}