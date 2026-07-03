using Trycore.Evm.Application.DTOs.Activities;

namespace Trycore.Evm.Application.DTOs.Evm;

public class ActivityEvmResponse
{
    public ActivityResponse Activity { get; set; } = new();

    public EvmMetricsResponse Metrics { get; set; } = new();
}