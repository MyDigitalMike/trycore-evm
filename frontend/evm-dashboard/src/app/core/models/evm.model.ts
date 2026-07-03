import { ActivityResponse } from './activity.model';
import { ProjectResponse } from './project.model';

export interface EvmMetricsResponse {
  bac: number;
  pv: number;
  ev: number;
  ac: number;
  cv: number;
  sv: number;
  cpi?: number | null;
  spi?: number | null;
  eac?: number | null;
  vac?: number | null;
  costStatus: string;
  scheduleStatus: string;
}

export interface ActivityEvmResponse {
  activity: ActivityResponse;
  metrics: EvmMetricsResponse;
}

export interface ProjectEvmResponse {
  project: ProjectResponse;
  summary: EvmMetricsResponse;
  activities: ActivityEvmResponse[];
}