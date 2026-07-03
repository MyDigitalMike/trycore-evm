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

        var cv = ev - ac;
        var sv = ev - pv;

        var cpi = ac == 0 ? (decimal?)null : ev / ac;
        var spi = pv == 0 ? (decimal?)null : ev / pv;

        var eac = cpi is null or 0 ? null : activity.Bac / cpi;
        var vac = eac is null ? null : activity.Bac - eac;

        return new EvmMetrics
        {
            Bac = activity.Bac,
            Pv = Round(pv),
            Ev = Round(ev),
            Ac = Round(ac),
            Cv = Round(cv),
            Sv = Round(sv),
            Cpi = RoundNullable(cpi),
            Spi = RoundNullable(spi),
            Eac = RoundNullable(eac),
            Vac = RoundNullable(vac),
            CostStatus = GetCostStatus(cpi),
            ScheduleStatus = GetScheduleStatus(spi)
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