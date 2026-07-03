export interface ActivityResponse {
  id: string;
  projectId: string;
  name: string;
  bac: number;
  plannedProgressPercent: number;
  actualProgressPercent: number;
  actualCost: number;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateActivityRequest {
  name: string;
  bac: number;
  plannedProgressPercent: number;
  actualProgressPercent: number;
  actualCost: number;
}

export interface UpdateActivityRequest {
  name: string;
  bac: number;
  plannedProgressPercent: number;
  actualProgressPercent: number;
  actualCost: number;
}