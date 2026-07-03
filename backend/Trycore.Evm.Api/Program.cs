using System.Reflection;
using Trycore.Evm.Application;
using Trycore.Evm.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddInfrastructure(builder.Configuration);
}

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Trycore EVM API",
        Version = "v1",
        Description = "REST API for managing projects, activities and Earned Value Management indicators."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevelopment", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Trycore EVM API v1");
        options.RoutePrefix = "swagger-ui";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularDevelopment");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}