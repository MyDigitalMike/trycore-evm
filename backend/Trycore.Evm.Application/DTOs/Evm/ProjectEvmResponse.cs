using Trycore.Evm.Application.DTOs.Activities;
using Trycore.Evm.Application.DTOs.Projects;

namespace Trycore.Evm.Application.DTOs.Evm;

public class ProjectEvmResponse
{
    public ProjectResponse Project { get; set; } = new();

    public EvmMetricsResponse Summary { get; set; } = new();

    public List<ActivityEvmResponse> Activities { get; set; } = [];
}