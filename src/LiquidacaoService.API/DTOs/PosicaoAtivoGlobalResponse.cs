namespace LiquidacaoService.API.DTOs;

public class PosicaoAtivoGlobalResponse
{
    public string CodigoAtivo { get; set; } = string.Empty;
    public decimal QuantidadeTotal { get; set; }
    public decimal ValorTotalInvestido { get; set; }
    public decimal PrecoMedioPonderado { get; set; }
}