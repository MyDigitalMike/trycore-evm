using Trycore.Evm.Application.DTOs.Activities;
using Trycore.Evm.Application.DTOs.Evm;
using Trycore.Evm.Application.DTOs.Projects;
using Trycore.Evm.Application.Interfaces;
using Trycore.Evm.Domain.Entities;
using Trycore.Evm.Domain.Services;
using Trycore.Evm.Domain.ValueObjects;

namespace Trycore.Evm.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly EvmCalculator _evmCalculator;

    public ProjectService(
        IProjectRepository projectRepository,
        EvmCalculator evmCalculator)
    {
        _projectRepository = projectRepository;
        _evmCalculator = evmCalculator;
    }

    public async Task<List<ProjectResponse>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();

        return projects
            .Select(MapProjectResponse)
            .ToList();
    }

    public async Task<ProjectResponse?> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        return project is null
            ? null
            : MapProjectResponse(project);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        var createdProject = await _projectRepository.AddAsync(project);

        return MapProjectResponse(createdProject);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProjectRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return false;

        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return false;

        await _projectRepository.DeleteAsync(project);

        return true;
    }

    public async Task<ActivityResponse?> AddActivityAsync(Guid projectId, CreateActivityRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        if (project is null)
            return null;

        var activity = new ProjectActivity
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = request.Name.Trim(),
            Bac = request.Bac,
            PlannedProgressPercent = request.PlannedProgressPercent,
            ActualProgressPercent = request.ActualProgressPercent,
            ActualCost = request.ActualCost,
            CreatedAt = DateTime.UtcNow
        };

        _evmCalculator.CalculateActivity(activity);

        var createdActivity = await _projectRepository.AddActivityAsync(activity);

        return MapActivityResponse(createdActivity);
    }

    public async Task<bool> UpdateActivityAsync(Guid projectId, Guid activityId, UpdateActivityRequest request)
    {
        var activity = await _projectRepository.GetActivityByIdAsync(projectId, activityId);

        if (activity is null)
            return false;

        activity.Name = request.Name.Trim();
        activity.Bac = request.Bac;
        activity.PlannedProgressPercent = request.PlannedProgressPercent;
        activity.ActualProgressPercent = request.ActualProgressPercent;
        activity.ActualCost = request.ActualCost;
        activity.UpdatedAt = DateTime.UtcNow;

        _evmCalculator.CalculateActivity(activity);

        await _projectRepository.UpdateActivityAsync(activity);

        return true;
    }

    public async Task<bool> DeleteActivityAsync(Guid projectId, Guid activityId)
    {
        var activity = await _projectRepository.GetActivityByIdAsync(projectId, activityId);

        if (activity is null)
            return false;

        await _projectRepository.DeleteActivityAsync(activity);

        return true;
    }

    public async Task<ProjectEvmResponse?> GetEvmReportAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdWithActivitiesAsync(projectId);

        if (project is null)
            return null;

        var summary = _evmCalculator.CalculateProject(project);

        var activities = project.Activities
            .Select(activity => new ActivityEvmResponse
            {
                Activity = MapActivityResponse(activity),
                Metrics = MapMetricsResponse(_evmCalculator.CalculateActivity(activity))
            })
            .ToList();

        return new ProjectEvmResponse
        {
            Project = MapProjectResponse(project),
            Summary = MapMetricsResponse(summary),
            Activities = activities
        };
    }

    private static ProjectResponse MapProjectResponse(Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    private static ActivityResponse MapActivityResponse(ProjectActivity activity)
    {
        return new ActivityResponse
        {
            Id = activity.Id,
            ProjectId = activity.ProjectId,
            Name = activity.Name,
            Bac = activity.Bac,
            PlannedProgressPercent = activity.PlannedProgressPercent,
            ActualProgressPercent = activity.ActualProgressPercent,
            ActualCost = activity.ActualCost,
            CreatedAt = activity.CreatedAt,
            UpdatedAt = activity.UpdatedAt
        };
    }

    private static EvmMetricsResponse MapMetricsResponse(EvmMetrics metrics)
    {
        return new EvmMetricsResponse
        {
            Bac = metrics.Bac,
            Pv = metrics.Pv,
            Ev = metrics.Ev,
            Ac = metrics.Ac,
            Cv = metrics.Cv,
            Sv = metrics.Sv,
            Cpi = metrics.Cpi,
            Spi = metrics.Spi,
            Eac = metrics.Eac,
            Vac = metrics.Vac,
            CostStatus = metrics.CostStatus,
            ScheduleStatus = metrics.ScheduleStatus
        };
    }
}