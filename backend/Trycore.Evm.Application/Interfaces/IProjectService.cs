using Trycore.Evm.Application.DTOs.Activities;
using Trycore.Evm.Application.DTOs.Evm;
using Trycore.Evm.Application.DTOs.Projects;

namespace Trycore.Evm.Application.Interfaces;

public interface IProjectService
{
    Task<List<ProjectResponse>> GetAllAsync();

    Task<ProjectResponse?> GetByIdAsync(Guid id);

    Task<ProjectResponse> CreateAsync(CreateProjectRequest request);

    Task<bool> UpdateAsync(Guid id, UpdateProjectRequest request);

    Task<bool> DeleteAsync(Guid id);

    Task<ActivityResponse?> AddActivityAsync(Guid projectId, CreateActivityRequest request);

    Task<bool> UpdateActivityAsync(Guid projectId, Guid activityId, UpdateActivityRequest request);

    Task<bool> DeleteActivityAsync(Guid projectId, Guid activityId);

    Task<ProjectEvmResponse?> GetEvmReportAsync(Guid projectId);
}