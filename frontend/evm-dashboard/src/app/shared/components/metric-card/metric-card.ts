import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
@Component({
  selector: 'app-metric-card',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './metric-card.html',
  styleUrl: './metric-card.scss',
})
export class MetricCard {
  title = input.required<string>();
  value = input.required<string | number>();
  subtitle = input<string>('');
}
