import { Component, input } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { ActivityEvmResponse } from '../../../../core/models/evm.model';
import { StatusBadge } from '../../../../shared/components/status-badge/status-badge';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [MatTableModule, StatusBadge],
  templateUrl: './activity-table.html',
  styleUrl: './activity-table.scss',
})
export class ActivityTable {
  activities = input.required<ActivityEvmResponse[]>();

  displayedColumns = [
    'name',
    'bac',
    'planned',
    'actual',
    'ac',
    'pv',
    'ev',
    'cpi',
    'spi',
    'status'
  ];

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }

  formatIndex(value: number | null | undefined): string {
    return value === null || value === undefined ? 'N/A' : value.toFixed(2);
  }
}
