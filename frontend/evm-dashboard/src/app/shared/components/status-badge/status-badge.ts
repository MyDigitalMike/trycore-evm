import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-status-badge',
  imports: [],
  standalone: true,
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.scss',
})
export class StatusBadge {
  status = input.required<string>();

  badgeClass = computed(() => {
    const value = this.status().toLowerCase();

    if (value.includes('under') || value.includes('ahead') || value.includes('on')) {
      return 'success';
    }

    if (value.includes('over') || value.includes('behind')) {
      return 'danger';
    }

    return 'neutral';
  });
}
