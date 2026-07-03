import { Component, input } from '@angular/core';
import { EvmMetricsResponse } from '../../../../core/models/evm.model';
import { MetricCard } from '../../../../shared/components/metric-card/metric-card';
import { StatusBadge } from '../../../../shared/components/status-badge/status-badge';

@Component({
  selector: 'app-evm-summary-cards',
  standalone: true,
  imports: [MetricCard, StatusBadge],
  templateUrl: './evm-summary-cards.html',
  styleUrl: './evm-summary-cards.scss',
})
export class EvmSummaryCards {
  summary = input.required<EvmMetricsResponse>();

  formatCurrency(value: number | null | undefined): string {
    if (value === null || value === undefined) {
      return 'N/A';
    }

    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'COP'
    }).format(value);
  }

  formatIndex(value: number | null | undefined): string {
    return value === null || value === undefined ? 'N/A' : value.toFixed(2);
  }
}
