import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  private baseUrl = 'http://localhost:5095/api';

  constructor(private http: HttpClient) {}

  getAdminSummary(): Observable<any> {
    return this.http.get<any>('http://localhost:5095/api/admin/summary?role=Admin');
  }

  getApplications(userId: number, role: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/JobApplications?userId=${userId}&role=${role}`);
  }

    getApplicationById(id: number, userId: number, role: string) {
    return this.http.get(`http://localhost:5095/api/JobApplications/${id}`, {
        params: { userId, role }
    });
    }

    getStatusLogs(applicationId: number) {
    return this.http.get(`/api/statuslogs/${applicationId}`);
    }

  getAllApplications(userId: number, role: string): Observable<{ data: any[] }> {
  if (role === 'Admin') {
    return this.http.get<{ data: any[] }>(`${this.baseUrl}/JobApplications`, {
      params: { role }
    });
  } else {
    return this.http.get<{ data: any[] }>(`${this.baseUrl}/JobApplications`, {
      params: { userId: userId.toString(), role }
    });
  }
}

createApplication(data: any) {
  return this.http.post('http://localhost:5095/api/JobApplications', data);
}

updateApplication(id: number, data: any) {
  return this.http.put(`http://localhost:5095/api/JobApplications/${id}`, data);
}

deleteApplication(id: number, userId: number, role: string) {
  return this.http.delete(`http://localhost:5095/api/JobApplications/${id}`, {
    params: { userId, role }
  });
}

getFilteredApplications(status?: string, companyName?: string): Observable<{ data: any[]; message: string }> {
  const role = localStorage.getItem('role') ?? '';
  const userId = role === 'Admin' ? 0 : Number(localStorage.getItem('userId'));

  const params: any = { role, userId };

  if (status?.trim()) params.status = status;
  if (companyName?.trim()) params.companyName = companyName;

  return this.http.get<{ data: any[]; message: string }>('http://localhost:5095/api/JobApplications/filter', { params });
}
  
}


