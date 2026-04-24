using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.API.DTOs;

public class CriarOperacaoRequest
{
    public Guid ClienteId { get; set; }
    public Guid AtivoId { get; set; }
    public TipoOperacao Tipo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public DateTime ExecutadaEm { get; set; }
}

public class OperacaoResponse
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public Guid AtivoId { get; set; }
    public string CodigoAtivo { get; set; } = string.Empty;
    public TipoOperacao Tipo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusOperacao Status { get; set; }
    public DateTime ExecutadaEm { get; set; }
    public LiquidacaoResponse? Liquidacao { get; set; }
}

public class LiquidacaoResponse
{
    public Guid Id { get; set; }
    public DateOnly DataPrevista { get; set; }
    public DateOnly? DataEfetiva { get; set; }
    public StatusLiquidacao Status { get; set; }
    public int Tentativas { get; set; }
    public string? Observacao { get; set; }
}

public class PosicaoAtivoResponse
{
    public string CodigoAtivo { get; set; } = string.Empty;
    public string DescricaoAtivo { get; set; } = string.Empty;
    public decimal QuantidadeTotal { get; set; }
    public decimal PrecoMedio { get; set; }
    public decimal ValorTotalInvestido { get; set; }
}

public class PosicaoClienteResponse
{
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public List<PosicaoAtivoResponse> Posicoes { get; set; } = new();
    public decimal ValorTotalCarteira { get; set; }
}