using Trycore.Evm.Domain.Entities;
using Trycore.Evm.Domain.Services;

namespace Trycore.Evm.Tests.Unit.Domain;

public class EvmCalculatorTests
{
    private readonly EvmCalculator _calculator = new();

    [Fact]
    public void CalculateActivity_ShouldCalculateEvmMetrics_WhenInputIsValid()
    {
        // Arrange
        var activity = new ProjectActivity
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Name = "Backend API development",
            Bac = 1000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 40m,
            ActualCost = 500m
        };

        // Act
        var result = _calculator.CalculateActivity(activity);

        // Assert
        Assert.Equal(500m, result.Pv);
        Assert.Equal(400m, result.Ev);
        Assert.Equal(-100m, result.Cv);
        Assert.Equal(-100m, result.Sv);
        Assert.Equal(0.8m, result.Cpi);
        Assert.Equal(0.8m, result.Spi);
        Assert.Equal(1250m, result.Eac);
        Assert.Equal(-250m, result.Vac);
        Assert.Equal("Over budget", result.CostStatus);
        Assert.Equal("Behind schedule", result.ScheduleStatus);
    }
    [Fact]
    public void CalculateActivity_ShouldReturnNullCpiAndEac_WhenActualCostIsZero()
    {
        var activity = new ProjectActivity
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Name = "Testing",
            Bac = 1000m,
            PlannedProgressPercent = 50m,
            ActualProgressPercent = 40m,
            ActualCost = 0m
        };

        var result = _calculator.CalculateActivity(activity);

        Assert.Equal(500m, result.Pv);
        Assert.Equal(400m, result.Ev);
        Assert.Equal(400m, result.Cv);
        Assert.Null(result.Cpi);
        Assert.Null(result.Eac);
        Assert.Null(result.Vac);
        Assert.Equal("No actual cost registered", result.CostStatus);
    }

    [Fact]
    public void CalculateActivity_ShouldReturnNullSpi_WhenPlannedValueIsZero()
    {
        var activity = new ProjectActivity
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Name = "Initial setup",
            Bac = 1000m,
            PlannedProgressPercent = 0m,
            ActualProgressPercent = 20m,
            ActualCost = 100m
        };

        var result = _calculator.CalculateActivity(activity);

        Assert.Equal(0m, result.Pv);
        Assert.Equal(200m, result.Ev);
        Assert.Null(result.Spi);
        Assert.Equal("No planned value registered", result.ScheduleStatus);
    }

    [Fact]
    public void CalculateActivity_ShouldReturnZeroEv_WhenActualProgressIsZero()
    {
        var activity = new ProjectActivity
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Name = "Frontend layout",
            Bac = 2000m,
            PlannedProgressPercent = 30m,
            ActualProgressPercent = 0m,
            ActualCost = 500m
        };

        var result = _calculator.CalculateActivity(activity);

        Assert.Equal(600m, result.Pv);
        Assert.Equal(0m, result.Ev);
        Assert.Equal(-500m, result.Cv);
        Assert.Equal(-600m, result.Sv);
        Assert.Equal(0m, result.Cpi);
        Assert.Equal(0m, result.Spi);
        Assert.Null(result.Eac);
        Assert.Null(result.Vac);
        Assert.Equal("Over budget", result.CostStatus);
        Assert.Equal("Behind schedule", result.ScheduleStatus);
    }

    [Fact]
    public void CalculateActivity_ShouldThrowException_WhenActivityIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _calculator.CalculateActivity(null!));
    }
}