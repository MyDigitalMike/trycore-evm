using System.Net;
using System.Net.Http.Json;
using Trycore.Evm.Application.DTOs.Activities;
using Trycore.Evm.Application.DTOs.Evm;
using Trycore.Evm.Application.DTOs.Projects;
using Trycore.Evm.Domain.Constants;

namespace Trycore.Evm.Tests.Integration.Api;

public class ProjectsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkWithProjectsList()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        await CreateProjectAsync(client);

        var response = await client.GetAsync("/api/projects");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var projects = await response.Content.ReadFromJsonAsync<List<ProjectResponse>>();

        Assert.NotNull(projects);
        Assert.NotEmpty(projects);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedProject()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var request = new CreateProjectRequest
        {
            Name = "Internal EVM Dashboard",
            Description = "Technical challenge project"
        };

        var response = await client.PostAsJsonAsync("/api/projects", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var project = await response.Content.ReadFromJsonAsync<ProjectResponse>();

        Assert.NotNull(project);
        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal(request.Name, project.Name);
        Assert.Equal(request.Description, project.Description);
    }

    [Fact]
    public async Task GetById_ShouldReturnProject_WhenProjectExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var createdProject = await CreateProjectAsync(client);

        var response = await client.GetAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var project = await response.Content.ReadFromJsonAsync<ProjectResponse>();

        Assert.NotNull(project);
        Assert.Equal(createdProject.Id, project.Id);
        Assert.Equal(createdProject.Name, project.Name);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenProjectExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var createdProject = await CreateProjectAsync(client);

        var request = new UpdateProjectRequest
        {
            Name = "Updated EVM Dashboard",
            Description = "Updated description"
        };

        var response = await client.PutAsJsonAsync($"/api/projects/{createdProject.Id}", request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await client.GetAsync($"/api/projects/{createdProject.Id}");
        var updatedProject = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        Assert.NotNull(updatedProject);
        Assert.Equal(request.Name, updatedProject.Name);
        Assert.Equal(request.Description, updatedProject.Description);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenProjectExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var createdProject = await CreateProjectAsync(client);

        var response = await client.DeleteAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await client.GetAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task AddActivity_ShouldReturnCreatedActivity_WhenProjectExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var project = await CreateProjectAsync(client);

        var request = new CreateActivityRequest
        {
            Name = "Backend API",
            Bac = 1000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 40m,
            ActualCost = 500m
        };

        var response = await client.PostAsJsonAsync($"/api/projects/{project.Id}/activities", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var activity = await response.Content.ReadFromJsonAsync<ActivityResponse>();

        Assert.NotNull(activity);
        Assert.NotEqual(Guid.Empty, activity.Id);
        Assert.Equal(project.Id, activity.ProjectId);
        Assert.Equal(request.Name, activity.Name);
        Assert.Equal(request.Bac, activity.Bac);
    }

    [Fact]
    public async Task UpdateActivity_ShouldReturnNoContent_WhenActivityExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var project = await CreateProjectAsync(client);
        var activity = await CreateActivityAsync(client, project.Id);

        var request = new UpdateActivityRequest
        {
            Name = "Updated Backend API",
            Bac = 1500m,
            PlannedProgressPercent = 60m,
            ActualProgressPercent = 50m,
            ActualCost = 700m
        };

        var response = await client.PutAsJsonAsync(
            $"/api/projects/{project.Id}/activities/{activity.Id}",
            request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var reportResponse = await client.GetAsync($"/api/projects/{project.Id}/evm");
        var report = await reportResponse.Content.ReadFromJsonAsync<ProjectEvmResponse>();

        Assert.NotNull(report);
        Assert.Contains(report.Activities, item => item.Activity.Name == request.Name);
    }

    [Fact]
    public async Task DeleteActivity_ShouldReturnNoContent_WhenActivityExists()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var project = await CreateProjectAsync(client);
        var activity = await CreateActivityAsync(client, project.Id);

        var response = await client.DeleteAsync($"/api/projects/{project.Id}/activities/{activity.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var reportResponse = await client.GetAsync($"/api/projects/{project.Id}/evm");
        var report = await reportResponse.Content.ReadFromJsonAsync<ProjectEvmResponse>();

        Assert.NotNull(report);
        Assert.Empty(report.Activities);
    }

    [Fact]
    public async Task GetEvmReport_ShouldReturnCalculatedSummary_WhenProjectHasActivities()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var project = await CreateProjectAsync(client);

        await CreateActivityAsync(client, project.Id, new CreateActivityRequest
        {
            Name = "Backend API",
            Bac = 1000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 40m,
            ActualCost = 500m
        });

        await CreateActivityAsync(client, project.Id, new CreateActivityRequest
        {
            Name = "Frontend dashboard",
            Bac = 2000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 80m,
            ActualCost = 1100m
        });

        await CreateActivityAsync(client, project.Id, new CreateActivityRequest
        {
            Name = "Testing",
            Bac = 1000m,
            PlannedProgressPercent = 25m,
            ActualProgressPercent = 50m,
            ActualCost = 400m
        });

        var response = await client.GetAsync($"/api/projects/{project.Id}/evm");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var report = await response.Content.ReadFromJsonAsync<ProjectEvmResponse>();

        Assert.NotNull(report);
        Assert.Equal(project.Id, report.Project.Id);
        Assert.Equal(3, report.Activities.Count);

        Assert.Equal(4000m, report.Summary.Bac);
        Assert.Equal(1750m, report.Summary.Pv);
        Assert.Equal(2500m, report.Summary.Ev);
        Assert.Equal(2000m, report.Summary.Ac);
        Assert.Equal(500m, report.Summary.Cv);
        Assert.Equal(750m, report.Summary.Sv);
        Assert.Equal(1.25m, report.Summary.Cpi);
        Assert.Equal(1.43m, report.Summary.Spi);
        Assert.Equal(3200m, report.Summary.Eac);
        Assert.Equal(800m, report.Summary.Vac);
        Assert.Equal(EvmStatusMessages.UnderBudget, report.Summary.CostStatus);
        Assert.Equal(EvmStatusMessages.AheadOfSchedule, report.Summary.ScheduleStatus);
    }

    [Fact]
    public async Task AddActivity_ShouldReturnBadRequest_WhenPercentagesAreInvalid()
    {
        using var factory = new EvmApiFactory();
        var client = factory.CreateClient();

        var project = await CreateProjectAsync(client);

        var request = new CreateActivityRequest
        {
            Name = "Invalid activity",
            Bac = 1000m,
            PlannedProgressPercent = 120m,
            ActualProgressPercent = 40m,
            ActualCost = 500m
        };

        var response = await client.PostAsJsonAsync($"/api/projects/{project.Id}/activities", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<ProjectResponse> CreateProjectAsync(HttpClient client)
    {
        var request = new CreateProjectRequest
        {
            Name = "Internal EVM Dashboard",
            Description = "Technical challenge project"
        };

        var response = await client.PostAsJsonAsync("/api/projects", request);

        response.EnsureSuccessStatusCode();

        var project = await response.Content.ReadFromJsonAsync<ProjectResponse>();

        Assert.NotNull(project);

        return project;
    }

    private static async Task<ActivityResponse> CreateActivityAsync(
        HttpClient client,
        Guid projectId)
    {
        var request = new CreateActivityRequest
        {
            Name = "Backend API",
            Bac = 1000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 40m,
            ActualCost = 500m
        };

        return await CreateActivityAsync(client, projectId, request);
    }

    private static async Task<ActivityResponse> CreateActivityAsync(
        HttpClient client,
        Guid projectId,
        CreateActivityRequest request)
    {
        var response = await client.PostAsJsonAsync(
            $"/api/projects/{projectId}/activities",
            request);

        response.EnsureSuccessStatusCode();

        var activity = await response.Content.ReadFromJsonAsync<ActivityResponse>();

        Assert.NotNull(activity);

        return activity;
    }
}