import { Component, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ProjectEvmResponse } from '../../../../core/models/evm.model';
import { ActivityTable } from '../../components/activity-table/activity-table';
import { EvmChart } from '../../components/evm-chart/evm-chart';
import { EvmSummaryCards } from '../../components/evm-summary-cards/evm-summary-cards';


@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [MatCardModule, EvmSummaryCards, ActivityTable, EvmChart],
  templateUrl: './project-dashboard.html',
  styleUrl: './project-dashboard.scss',
})
export class ProjectDashboard {
   report = signal<ProjectEvmResponse>({
    project: {
      id: 'mock-project-id',
      name: 'Internal EVM Dashboard',
      description: 'Technical challenge project for Trycore',
      createdAt: new Date().toISOString()
    },
    summary: {
      bac: 4000,
      pv: 1750,
      ev: 2500,
      ac: 2000,
      cv: 500,
      sv: 750,
      cpi: 1.25,
      spi: 1.43,
      eac: 3200,
      vac: 800,
      costStatus: 'Under budget',
      scheduleStatus: 'Ahead of schedule'
    },
    activities: [
      {
        activity: {
          id: '1',
          projectId: 'mock-project-id',
          name: 'Backend API',
          bac: 1000,
          plannedProgressPercent: 50,
          actualProgressPercent: 40,
          actualCost: 500,
          createdAt: new Date().toISOString()
        },
        metrics: {
          bac: 1000,
          pv: 500,
          ev: 400,
          ac: 500,
          cv: -100,
          sv: -100,
          cpi: 0.8,
          spi: 0.8,
          eac: 1250,
          vac: -250,
          costStatus: 'Over budget',
          scheduleStatus: 'Behind schedule'
        }
      },
      {
        activity: {
          id: '2',
          projectId: 'mock-project-id',
          name: 'Frontend dashboard',
          bac: 2000,
          plannedProgressPercent: 50,
          actualProgressPercent: 80,
          actualCost: 1100,
          createdAt: new Date().toISOString()
        },
        metrics: {
          bac: 2000,
          pv: 1000,
          ev: 1600,
          ac: 1100,
          cv: 500,
          sv: 600,
          cpi: 1.45,
          spi: 1.6,
          eac: 1379.31,
          vac: 620.69,
          costStatus: 'Under budget',
          scheduleStatus: 'Ahead of schedule'
        }
      },
      {
        activity: {
          id: '3',
          projectId: 'mock-project-id',
          name: 'Testing',
          bac: 1000,
          plannedProgressPercent: 25,
          actualProgressPercent: 50,
          actualCost: 400,
          createdAt: new Date().toISOString()
        },
        metrics: {
          bac: 1000,
          pv: 250,
          ev: 500,
          ac: 400,
          cv: 100,
          sv: 250,
          cpi: 1.25,
          spi: 2,
          eac: 800,
          vac: 200,
          costStatus: 'Under budget',
          scheduleStatus: 'Ahead of schedule'
        }
      }
    ]
  });
}
