using Trycore.Evm.Domain.Entities;

namespace Trycore.Evm.Application.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(Guid id);

    Task<Project?> GetByIdWithActivitiesAsync(Guid id);

    Task<Project> AddAsync(Project project);

    Task UpdateAsync(Project project);

    Task DeleteAsync(Project project);

    Task<ProjectActivity?> GetActivityByIdAsync(Guid projectId, Guid activityId);

    Task<ProjectActivity> AddActivityAsync(ProjectActivity activity);

    Task UpdateActivityAsync(ProjectActivity activity);

    Task DeleteActivityAsync(ProjectActivity activity);
}