using Microsoft.AspNetCore.Mvc;
using Trycore.Evm.Application.DTOs.Activities;
using Trycore.Evm.Application.DTOs.Evm;
using Trycore.Evm.Application.DTOs.Projects;
using Trycore.Evm.Application.Interfaces;

namespace Trycore.Evm.Api.Controllers;

[ApiController]
[Route("api/projects")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        IProjectService projectService,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all registered projects.
    /// </summary>
    /// <returns>List of projects.</returns>
    /// <response code="200">Returns the list of projects.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
    {
        var projects = await _projectService.GetAllAsync();

        return Ok(projects);
    }

    /// <summary>
    /// Gets a project by its identifier.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Project information.</returns>
    /// <response code="200">Returns the requested project.</response>
    /// <response code="404">Project was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project is null)
            return NotFound();

        return Ok(project);
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="request">Project creation request.</param>
    /// <returns>Created project.</returns>
    /// <response code="201">Project was created successfully.</response>
    /// <response code="400">Request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Project name is required.");

        var project = await _projectService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="request">Project update request.</param>
    /// <response code="204">Project was updated successfully.</response>
    /// <response code="400">Request data is invalid.</response>
    /// <response code="404">Project was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Project name is required.");

        var updated = await _projectService.UpdateAsync(id, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <response code="204">Project was deleted successfully.</response>
    /// <response code="404">Project was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _projectService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Adds an activity to a project.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="request">Activity creation request.</param>
    /// <returns>Created activity.</returns>
    /// <response code="201">Activity was created successfully.</response>
    /// <response code="400">Request data is invalid.</response>
    /// <response code="404">Project was not found.</response>
    [HttpPost("{projectId:guid}/activities")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityResponse>> AddActivity(
        Guid projectId,
        [FromBody] CreateActivityRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Activity name is required.");

            var activity = await _projectService.AddActivityAsync(projectId, request);

            if (activity is null)
                return NotFound();

            return CreatedAtAction(
                nameof(GetEvmReport),
                new { projectId },
                activity
            );
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning(ex, "Invalid activity data.");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing project activity.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="activityId">Activity identifier.</param>
    /// <param name="request">Activity update request.</param>
    /// <response code="204">Activity was updated successfully.</response>
    /// <response code="400">Request data is invalid.</response>
    /// <response code="404">Activity was not found.</response>
    [HttpPut("{projectId:guid}/activities/{activityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateActivity(
        Guid projectId,
        Guid activityId,
        [FromBody] UpdateActivityRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Activity name is required.");

            var updated = await _projectService.UpdateActivityAsync(projectId, activityId, request);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning(ex, "Invalid activity data.");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an existing project activity.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="activityId">Activity identifier.</param>
    /// <response code="204">Activity was deleted successfully.</response>
    /// <response code="404">Activity was not found.</response>
    [HttpDelete("{projectId:guid}/activities/{activityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(Guid projectId, Guid activityId)
    {
        var deleted = await _projectService.DeleteActivityAsync(projectId, activityId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Gets the Earned Value Management report for a project.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Project EVM summary and activity-level EVM indicators.</returns>
    /// <response code="200">Returns project EVM indicators.</response>
    /// <response code="404">Project was not found.</response>
    [HttpGet("{projectId:guid}/evm")]
    [ProducesResponseType(typeof(ProjectEvmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectEvmResponse>> GetEvmReport(Guid projectId)
    {
        var report = await _projectService.GetEvmReportAsync(projectId);

        if (report is null)
            return NotFound();

        return Ok(report);
    }
}