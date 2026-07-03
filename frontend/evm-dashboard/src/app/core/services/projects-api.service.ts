import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ActivityResponse, CreateActivityRequest, UpdateActivityRequest } from '../models/activity.model';
import { ProjectEvmResponse } from '../models/evm.model';
import { CreateProjectRequest, ProjectResponse } from '../models/project.model';
import { API_BASE_URL } from './api.config';

@Injectable({
  providedIn: 'root'
})
export class ProjectsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/projects`;

  getProjects(): Observable<ProjectResponse[]> {
    return this.http.get<ProjectResponse[]>(this.baseUrl);
  }

  createProject(request: CreateProjectRequest): Observable<ProjectResponse> {
    return this.http.post<ProjectResponse>(this.baseUrl, request);
  }

  getEvmReport(projectId: string): Observable<ProjectEvmResponse> {
    return this.http.get<ProjectEvmResponse>(`${this.baseUrl}/${projectId}/evm`);
  }

  addActivity(projectId: string, request: CreateActivityRequest): Observable<ActivityResponse> {
    return this.http.post<ActivityResponse>(`${this.baseUrl}/${projectId}/activities`, request);
  }

  updateActivity(
    projectId: string,
    activityId: string,
    request: UpdateActivityRequest
  ): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${projectId}/activities/${activityId}`, request);
  }

  deleteActivity(projectId: string, activityId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${projectId}/activities/${activityId}`);
  }
}