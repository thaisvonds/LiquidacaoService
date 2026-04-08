using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.API.Domain.Entities;

public class Operacao
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public Guid AtivoId { get; set; }
    public Ativo Ativo { get; set; } = null!;
    public TipoOperacao Tipo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusOperacao Status { get; set; } = StatusOperacao.Pendente;
    public DateTime ExecutadaEm { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public Liquidacao? Liquidacao { get; set; }
}