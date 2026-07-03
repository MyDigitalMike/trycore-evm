namespace Trycore.Evm.Application.DTOs.Activities;

public class CreateActivityRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Bac { get; set; }

    public decimal PlannedProgressPercent { get; set; }

    public decimal ActualProgressPercent { get; set; }

    public decimal ActualCost { get; set; }
}