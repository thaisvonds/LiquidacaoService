namespace LiquidacaoService.API.Domain.Entities;

public class FeriadoCalendario
{
    public Guid Id { get; set; }
    public DateOnly Data { get; set; }
    public string Nome { get; set; } = string.Empty;
}