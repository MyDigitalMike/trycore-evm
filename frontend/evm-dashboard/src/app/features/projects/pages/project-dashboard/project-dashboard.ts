import { Component, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

import { CreateActivityRequest } from '../../../../core/models/activity.model';
import { ProjectEvmResponse } from '../../../../core/models/evm.model';
import { ProjectResponse } from '../../../../core/models/project.model';
import { ProjectsApiService } from '../../../../core/services/projects-api.service';
import { ActivityForm } from '../../components/activity-form/activity-form';
import { ActivityTable } from '../../components/activity-table/activity-table';
import { EvmChart } from '../../components/evm-chart/evm-chart';
import { EvmSummaryCards } from '../../components/evm-summary-cards/evm-summary-cards';

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    EvmSummaryCards,
    ActivityTable,
    EvmChart,
    ActivityForm
  ],
  templateUrl: './project-dashboard.html',
  styleUrl: './project-dashboard.scss'
})
export class ProjectDashboard implements OnInit {
  private readonly projectsApi = inject(ProjectsApiService);

  projects = signal<ProjectResponse[]>([]);
  selectedProjectId = signal<string | null>(null);
  report = signal<ProjectEvmResponse | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.loading.set(true);
    this.error.set(null);

    this.projectsApi.getProjects().subscribe({
      next: (projects) => {
        this.projects.set(projects);

        if (projects.length > 0) {
          this.selectedProjectId.set(projects[0].id);
          this.loadReport(projects[0].id);
          return;
        }

        this.createDemoProject();
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Could not load projects. Check that the backend API is running.');
      }
    });
  }

  createDemoProject(): void {
    this.projectsApi.createProject({
      name: 'Internal EVM Dashboard',
      description: 'Technical challenge project for Trycore'
    }).subscribe({
      next: (project) => {
        this.projects.set([project]);
        this.selectedProjectId.set(project.id);
        this.loadReport(project.id);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Could not create demo project.');
      }
    });
  }

  loadReport(projectId: string): void {
    this.loading.set(true);
    this.error.set(null);

    this.projectsApi.getEvmReport(projectId).subscribe({
      next: (report) => {
        this.report.set(report);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Could not load EVM report.');
      }
    });
  }

  addActivity(request: CreateActivityRequest): void {
    const projectId = this.selectedProjectId();

    if (!projectId) {
      this.error.set('There is no selected project.');
      return;
    }

    this.projectsApi.addActivity(projectId, request).subscribe({
      next: () => {
        this.loadReport(projectId);
      },
      error: () => {
        this.error.set('Could not create activity. Check input values.');
      }
    });
  }
}