import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

import { ActivityResponse } from '../../../../core/models/activity.model';
import { ActivityEvmResponse } from '../../../../core/models/evm.model';
import { StatusBadge } from '../../../../shared/components/status-badge/status-badge';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, StatusBadge],
  templateUrl: './activity-table.html',
  styleUrl: './activity-table.scss'
})
export class ActivityTable {
  activities = input.required<ActivityEvmResponse[]>();

  editRequested = output<ActivityResponse>();
  deleteRequested = output<ActivityResponse>();

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
    'status',
    'actions'
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

  edit(activity: ActivityResponse): void {
    this.editRequested.emit(activity);
  }

  delete(activity: ActivityResponse): void {
    this.deleteRequested.emit(activity);
  }
}