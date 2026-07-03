namespace Trycore.Evm.Domain.ValueObjects;

public class EvmMetrics
{
    public decimal Bac { get; set; }

    public decimal Pv { get; set; }

    public decimal Ev { get; set; }

    public decimal Ac { get; set; }

    public decimal Cv { get; set; }

    public decimal Sv { get; set; }

    public decimal? Cpi { get; set; }

    public decimal? Spi { get; set; }

    public decimal? Eac { get; set; }

    public decimal? Vac { get; set; }

    public string CostStatus { get; set; } = string.Empty;

    public string ScheduleStatus { get; set; } = string.Empty;
}