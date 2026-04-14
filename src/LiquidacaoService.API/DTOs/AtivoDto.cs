using LiquidacaoService.API.Domain.Enums;

namespace LiquidacaoService.API.DTOs;

public class CriarAtivoRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoAtivo Tipo { get; set; }
    public int PrazoLiquidacao { get; set; }
}

public class AtivoResponse
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoAtivo Tipo { get; set; }
    public int PrazoLiquidacao { get; set; }
}