using Microsoft.EntityFrameworkCore;
using Trycore.Evm.Application.Interfaces;
using Trycore.Evm.Domain.Entities;
using Trycore.Evm.Infrastructure.Persistence;

namespace Trycore.Evm.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly EvmDbContext _context;

    public ProjectRepository(EvmDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderBy(project => project.Name)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(project => project.Id == id);
    }

    public async Task<Project?> GetByIdWithActivitiesAsync(Guid id)
    {
        return await _context.Projects
            .AsNoTracking()
            .Include(project => project.Activities)
            .FirstOrDefaultAsync(project => project.Id == id);
    }

    public async Task<Project> AddAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project;
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectActivity?> GetActivityByIdAsync(Guid projectId, Guid activityId)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(activity =>
                activity.ProjectId == projectId &&
                activity.Id == activityId);
    }

    public async Task<ProjectActivity> AddActivityAsync(ProjectActivity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return activity;
    }

    public async Task UpdateActivityAsync(ProjectActivity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteActivityAsync(ProjectActivity activity)
    {
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
    }
}