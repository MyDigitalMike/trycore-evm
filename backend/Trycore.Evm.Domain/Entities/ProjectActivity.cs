namespace Trycore.Evm.Domain.Entities;

public class ProjectActivity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Bac { get; set; }

    public decimal PlannedProgressPercent { get; set; }

    public decimal ActualProgressPercent { get; set; }

    public decimal ActualCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Project? Project { get; set; }
}