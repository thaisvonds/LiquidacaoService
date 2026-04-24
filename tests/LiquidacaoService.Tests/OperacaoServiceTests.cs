using Moq;
using Xunit;
using LiquidacaoService.API.Services;
using LiquidacaoService.API.Repositories;
using LiquidacaoService.API.Domain.Entities;
using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.Tests;

public class OperacaoServiceTests
{
   [Fact]
public async Task Criar_DeveCalcularDataLiquidacaoCorretamente_Dmais2()
{
    // Arrange
    var repoOperacao = new Mock<IOperacaoRepository>();
    var repoCliente = new Mock<IClienteRepository>();
    var repoAtivo = new Mock<IAtivoRepository>();
    var serviceCalendario = new Mock<ICalendarioService>();

    var service = new OperacaoService(repoOperacao.Object, repoCliente.Object, repoAtivo.Object, serviceCalendario.Object);

    var clienteId = Guid.NewGuid();
    var ativoId = Guid.NewGuid();
    var dataExecucao = new DateTime(2026, 4, 20);
    var dataLiquidacaoEsperada = new DateOnly(2026, 4, 22);

    var clienteFake = new Cliente { Id = clienteId, Nome = "Thais Teste" };
    var ativoFake = new Ativo { Id = ativoId, Codigo = "PETR4", PrazoLiquidacao = 2 };

    repoCliente.Setup(r => r.BuscarPorId(clienteId)).ReturnsAsync(clienteFake);
    repoAtivo.Setup(r => r.BuscarPorId(ativoId)).ReturnsAsync(ativoFake);
    serviceCalendario.Setup(s => s.AdicionarDiasUteis(It.IsAny<DateOnly>(), 2)).ReturnsAsync(dataLiquidacaoEsperada);

    // IMPORTANTE: O seu método Criar faz um BuscarPorId no final. 
    // Precisamos configurar o Mock para devolver a operação com o Ativo e Cliente preenchidos!
    repoOperacao.Setup(r => r.BuscarPorId(It.IsAny<Guid>()))
        .ReturnsAsync(new Operacao 
        { 
            Id = Guid.NewGuid(), 
            Ativo = ativoFake, 
            Cliente = clienteFake,
            Liquidacao = new Liquidacao { DataPrevista = dataLiquidacaoEsperada }
        });

    var request = new CriarOperacaoRequest { ClienteId = clienteId, AtivoId = ativoId, Quantidade = 10, PrecoUnitario = 30, ExecutadaEm = dataExecucao };

    // Act
    var resultado = await service.Criar(request);

    // Assert
    Assert.NotNull(resultado);
    Assert.Equal(dataLiquidacaoEsperada, resultado.Liquidacao.DataPrevista);
}
}