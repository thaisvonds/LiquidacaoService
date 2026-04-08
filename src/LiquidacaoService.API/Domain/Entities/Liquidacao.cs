using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.API.Domain.Entities;

public class Liquidacao
{
    public Guid Id { get; set; }
    public Guid OperacaoId { get; set; }
    public Operacao Operacao { get; set; } = null!;
    public DateOnly DataPrevista { get; set; }
    public DateOnly? DataEfetiva { get; set; }
    public StatusLiquidacao Status { get; set; } = StatusLiquidacao.Pendente;
    public int Tentativas { get; set; } = 0;
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}