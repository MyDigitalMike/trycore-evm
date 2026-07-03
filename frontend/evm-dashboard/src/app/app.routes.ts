import { Routes } from '@angular/router';
import { ProjectDashboard } from './features/projects/pages/project-dashboard/project-dashboard';

export const routes: Routes = [
  {
    path: '',
    component: ProjectDashboard
  },
  {
    path: '**',
    redirectTo: ''
  }
];