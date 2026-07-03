using Trycore.Evm.Domain.Entities;
using Trycore.Evm.Domain.ValueObjects;

namespace Trycore.Evm.Domain.Services;

public class EvmCalculator
{
    public EvmMetrics CalculateActivity(ProjectActivity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        var pv = CalculatePercentageValue(activity.PlannedProgressPercent, activity.Bac);
        var ev = CalculatePercentageValue(activity.ActualProgressPercent, activity.Bac);
        var ac = activity.ActualCost;

        return BuildMetrics(
            bac: activity.Bac,
            pv: pv,
            ev: ev,
            ac: ac,
            emptyStatus: null
        );
    }

    public EvmMetrics CalculateProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (project.Activities.Count == 0)
        {
            return BuildMetrics(
                bac: 0m,
                pv: 0m,
                ev: 0m,
                ac: 0m,
                emptyStatus: "No activities registered"
            );
        }

        var activityMetrics = project.Activities
            .Select(CalculateActivity)
            .ToList();

        var totalBac = activityMetrics.Sum(x => x.Bac);
        var totalPv = activityMetrics.Sum(x => x.Pv);
        var totalEv = activityMetrics.Sum(x => x.Ev);
        var totalAc = activityMetrics.Sum(x => x.Ac);

        return BuildMetrics(
            bac: totalBac,
            pv: totalPv,
            ev: totalEv,
            ac: totalAc,
            emptyStatus: null
        );
    }

    private static EvmMetrics BuildMetrics(
        decimal bac,
        decimal pv,
        decimal ev,
        decimal ac,
        string? emptyStatus)
    {
        var cv = ev - ac;
        var sv = ev - pv;

        decimal? cpi = ac == 0 ? null : ev / ac;
        decimal? spi = pv == 0 ? null : ev / pv;

        var eac = cpi is null or 0 ? null : bac / cpi;
        var vac = eac is null ? null : bac - eac;

        return new EvmMetrics
        {
            Bac = Round(bac),
            Pv = Round(pv),
            Ev = Round(ev),
            Ac = Round(ac),
            Cv = Round(cv),
            Sv = Round(sv),
            Cpi = RoundNullable(cpi),
            Spi = RoundNullable(spi),
            Eac = RoundNullable(eac),
            Vac = RoundNullable(vac),
            CostStatus = emptyStatus ?? GetCostStatus(cpi),
            ScheduleStatus = emptyStatus ?? GetScheduleStatus(spi)
        };
    }

    private static decimal CalculatePercentageValue(decimal percent, decimal bac)
    {
        return bac * (percent / 100m);
    }

    private static string GetCostStatus(decimal? cpi)
    {
        if (cpi is null)
            return "No actual cost registered";

        if (cpi > 1)
            return "Under budget";

        if (cpi < 1)
            return "Over budget";

        return "On budget";
    }

    private static string GetScheduleStatus(decimal? spi)
    {
        if (spi is null)
            return "No planned value registered";

        if (spi > 1)
            return "Ahead of schedule";

        if (spi < 1)
            return "Behind schedule";

        return "On schedule";
    }

    private static decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal? RoundNullable(decimal? value)
    {
        return value.HasValue
            ? Round(value.Value)
            : null;
    }
}