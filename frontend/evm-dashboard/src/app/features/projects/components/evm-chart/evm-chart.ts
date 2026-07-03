import {
  AfterViewInit,
  Component,
  effect,
  ElementRef,
  input,
  OnDestroy,
  ViewChild
} from '@angular/core';
import { Chart } from 'chart.js/auto';
import { ActivityEvmResponse } from '../../../../core/models/evm.model';

@Component({
  selector: 'app-evm-chart',
  standalone: true,
  templateUrl: './evm-chart.html',
  styleUrl: './evm-chart.scss'
})
export class EvmChart implements AfterViewInit, OnDestroy {
  activities = input.required<ActivityEvmResponse[]>();

  @ViewChild('chartCanvas')
  chartCanvas?: ElementRef<HTMLCanvasElement>;

  private chart?: Chart;
  private viewReady = false;

  constructor() {
    effect(() => {
      this.activities();

      if (this.viewReady) {
        this.renderChart();
      }
    });
  }

  ngAfterViewInit(): void {
    this.viewReady = true;
    this.renderChart();
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }

  private renderChart(): void {
    if (!this.chartCanvas) {
      return;
    }

    this.chart?.destroy();

    const labels = this.activities().map(item => item.activity.name);

    this.chart = new Chart(this.chartCanvas.nativeElement, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'PV',
            data: this.activities().map(item => item.metrics.pv)
          },
          {
            label: 'EV',
            data: this.activities().map(item => item.metrics.ev)
          },
          {
            label: 'AC',
            data: this.activities().map(item => item.metrics.ac)
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom'
          }
        },
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}