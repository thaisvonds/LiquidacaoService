using LiquidacaoService.API.Domain.Entities;
using LiquidacaoService.API.Domain.Enums;
using LiquidacaoService.API.DTOs;
using LiquidacaoService.API.Repositories;

namespace LiquidacaoService.API.Services;

public class OperacaoService : IOperacaoService
{
    private static readonly Guid ClienteTesteId = Guid.Parse("3b14a17a-ccfd-477a-8794-c9e0e501fb45");

    private readonly IOperacaoRepository _operacaoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IAtivoRepository _ativoRepository;
    private readonly ICalendarioService _calendarioService;

    public OperacaoService(
        IOperacaoRepository operacaoRepository,
        IClienteRepository clienteRepository,
        IAtivoRepository ativoRepository,
        ICalendarioService calendarioService)
    {
        _operacaoRepository = operacaoRepository;
        _clienteRepository = clienteRepository;
        _ativoRepository = ativoRepository;
        _calendarioService = calendarioService;
    }

    public async Task<IEnumerable<OperacaoResponse>> BuscarTodos()
    {
        var operacoes = await _operacaoRepository.BuscarTodos();
        return operacoes.Select(o => ParaResponse(o));
    }

    public async Task<IEnumerable<OperacaoResponse>> BuscarPorCliente(Guid clienteId)
    {
        var operacoes = await _operacaoRepository.BuscarPorCliente(clienteId);
        return operacoes.Select(o => ParaResponse(o));
    }

    public async Task<OperacaoResponse?> BuscarPorId(Guid id)
    {
        var operacao = await _operacaoRepository.BuscarPorId(id);
        if (operacao is null) return null;
        return ParaResponse(operacao);
    }

    public async Task<OperacaoResponse> Criar(CriarOperacaoRequest request)
    {
        var cliente = await _clienteRepository.BuscarPorId(request.ClienteId)
            ?? throw new Exception("Cliente não encontrado.");

        var ativo = await _ativoRepository.BuscarPorId(request.AtivoId)
            ?? throw new Exception("Ativo não encontrado.");

        var valorTotal = request.Quantidade * request.PrecoUnitario;

        var dataBase = DateOnly.FromDateTime(request.ExecutadaEm);
        var dataLiquidacao = await _calendarioService
            .AdicionarDiasUteis(dataBase, ativo.PrazoLiquidacao);

        var operacao = new Operacao
        {
            Id = Guid.NewGuid(),
            ClienteId = cliente.Id,
            AtivoId = ativo.Id,
            Tipo = request.Tipo,
            Quantidade = request.Quantidade,
            PrecoUnitario = request.PrecoUnitario,
            ValorTotal = valorTotal,
            Status = StatusOperacao.Pendente,
            ExecutadaEm = request.ExecutadaEm,
            Liquidacao = new Liquidacao
            {
                Id = Guid.NewGuid(),
                DataPrevista = dataLiquidacao,
                Status = StatusLiquidacao.Pendente
            }
        };

        await _operacaoRepository.Adicionar(operacao);
        await _operacaoRepository.Salvar();

        var operacaoCriada = await _operacaoRepository.BuscarPorId(operacao.Id);
        return ParaResponse(operacaoCriada!);
    }

    public async Task GerarMassaDeTeste()
    {
        var cliente = await _clienteRepository.BuscarPorId(ClienteTesteId)
            ?? throw new Exception("Cliente de teste não encontrado.");

        var operacoesTeste = new[]
        {
            new { CodigoAtivo = "PETR4", Quantidade = 100m, PrecoUnitario = 31.45m },
            new { CodigoAtivo = "VALE3", Quantidade = 80m, PrecoUnitario = 58.90m },
            new { CodigoAtivo = "ITUB4", Quantidade = 120m, PrecoUnitario = 34.10m }
        };

        foreach (var operacaoTeste in operacoesTeste)
        {
            var ativo = await _ativoRepository.BuscarPorCodigo(operacaoTeste.CodigoAtivo)
                ?? throw new Exception($"Ativo {operacaoTeste.CodigoAtivo} não encontrado.");

            var executadaEm = DateTime.UtcNow;
            var dataLiquidacao = await _calendarioService
                .AdicionarDiasUteis(DateOnly.FromDateTime(executadaEm), ativo.PrazoLiquidacao);

            var operacao = new Operacao
            {
                Id = Guid.NewGuid(),
                ClienteId = cliente.Id,
                AtivoId = ativo.Id,
                Tipo = TipoOperacao.Compra,
                Quantidade = operacaoTeste.Quantidade,
                PrecoUnitario = operacaoTeste.PrecoUnitario,
                ValorTotal = operacaoTeste.Quantidade * operacaoTeste.PrecoUnitario,
                Status = StatusOperacao.Pendente,
                ExecutadaEm = executadaEm,
                Liquidacao = new Liquidacao
                {
                    Id = Guid.NewGuid(),
                    DataPrevista = dataLiquidacao,
                    Status = StatusLiquidacao.Pendente
                }
            };

            await _operacaoRepository.Adicionar(operacao);
        }

        await _operacaoRepository.Salvar();
    }

    public async Task<OperacaoResponse> ProcessarLiquidacao(Guid operacaoId)
    {
        var operacao = await _operacaoRepository.BuscarPorId(operacaoId)
            ?? throw new Exception("Operação não encontrada.");

        if (operacao.Liquidacao is null)
            throw new Exception("Liquidação não encontrada.");

        if (operacao.Status == StatusOperacao.Liquidada)
            throw new Exception("Operação já foi liquidada.");

        operacao.Liquidacao.Tentativas++;
        operacao.Status = StatusOperacao.Liquidada;
        operacao.Liquidacao.Status = StatusLiquidacao.Liquidada;
        operacao.Liquidacao.DataEfetiva = DateOnly.FromDateTime(DateTime.UtcNow);

        await _operacaoRepository.Salvar();
        return ParaResponse(operacao);
    }

    public async Task<IEnumerable<OperacaoResponse>> BuscarPendentes()
    {
        var operacoes = await _operacaoRepository.BuscarPendentes();
        return operacoes.Select(o => ParaResponse(o));
    }

    public async Task<PosicaoClienteResponse> BuscarPosicaoCliente(Guid clienteId)
    {
        var cliente = await _clienteRepository.BuscarPorId(clienteId)
            ?? throw new Exception("Cliente não encontrado.");

        var operacoes = await _operacaoRepository.BuscarPorCliente(clienteId);

        var operacoesLiquidadas = operacoes
            .Where(o => o.Status == StatusOperacao.Liquidada &&
                        o.Tipo == TipoOperacao.Compra)
            .ToList();

        var posicoes = operacoesLiquidadas
            .GroupBy(o => o.AtivoId)
            .Select(grupo =>
            {
                var quantidadeTotal = grupo.Sum(o => o.Quantidade);
                var valorTotalInvestido = grupo.Sum(o => o.ValorTotal);
                var precoMedio = quantidadeTotal > 0
                    ? valorTotalInvestido / quantidadeTotal
                    : 0;

                return new PosicaoAtivoResponse
                {
                    CodigoAtivo = grupo.First().Ativo?.Codigo ?? "",
                    DescricaoAtivo = grupo.First().Ativo?.Descricao ?? "",
                    QuantidadeTotal = quantidadeTotal,
                    PrecoMedio = Math.Round(precoMedio, 2),
                    ValorTotalInvestido = Math.Round(valorTotalInvestido, 2)
                };
            })
            .ToList();

        return new PosicaoClienteResponse
        {
            ClienteId = clienteId,
            NomeCliente = cliente.Nome,
            Posicoes = posicoes,
            ValorTotalCarteira = posicoes.Sum(p => p.ValorTotalInvestido)
        };
    }

    public async Task<IEnumerable<PosicaoAtivoGlobalResponse>> ObterPosicaoGlobal()
    {
        var operacoes = await _operacaoRepository.BuscarTodasLiquidadas();

        // Agrupa por código do ativo e calcula os totais
        var posicaoGlobal = operacoes
            .GroupBy(o => o.Ativo.Codigo)
            .Select(g => new PosicaoAtivoGlobalResponse
            {
                CodigoAtivo = g.Key,
                QuantidadeTotal = g.Sum(o => o.Quantidade),
                ValorTotalInvestido = g.Sum(o => o.ValorTotal),
                PrecoMedioPonderado = g.Sum(o => o.ValorTotal) / g.Sum(o => o.Quantidade)
            })
            .OrderBy(p => p.CodigoAtivo);

        return posicaoGlobal;
    } 

    private OperacaoResponse ParaResponse(Operacao operacao)
    {
        return new OperacaoResponse
        {
            Id = operacao.Id,
            ClienteId = operacao.ClienteId,
            NomeCliente = operacao.Cliente?.Nome ?? "",
            AtivoId = operacao.AtivoId,
            CodigoAtivo = operacao.Ativo?.Codigo ?? "",
            Tipo = operacao.Tipo,
            Quantidade = operacao.Quantidade,
            PrecoUnitario = operacao.PrecoUnitario,
            ValorTotal = operacao.ValorTotal,
            Status = operacao.Status,
            ExecutadaEm = operacao.ExecutadaEm,
            Liquidacao = operacao.Liquidacao is null ? null : new LiquidacaoResponse
            {
                Id = operacao.Liquidacao.Id,
                DataPrevista = operacao.Liquidacao.DataPrevista,
                DataEfetiva = operacao.Liquidacao.DataEfetiva,
                Status = operacao.Liquidacao.Status,
                Tentativas = operacao.Liquidacao.Tentativas,
                Observacao = operacao.Liquidacao.Observacao
            }
        };
    }
}