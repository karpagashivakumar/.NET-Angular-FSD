import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AssetAudit } from '../models/asset-audit.model';

@Injectable({ providedIn: 'root' })
export class AssetAuditService {
  private baseUrl = 'http://localhost:5116/api/v1/AssetAudit';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('jwtToken');
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getMyAudits(): Observable<AssetAudit[]> {
  return this.http.get<AssetAudit[]>(`${this.baseUrl}/my-audits`, {
    headers: this.getHeaders()
  });
}

  getPending(): Observable<AssetAudit[]> {
    return this.http.get<AssetAudit[]>(`${this.baseUrl}/pending`, { headers: this.getHeaders() });
  }

  createAudit(dto: { assetId: number; employeeId: number; auditNotes?: string }): Observable<any> {
    return this.http.post(`${this.baseUrl}/with-dto`, dto, { headers: this.getHeaders() });
  }

  completeAudit(id: number, notes: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/complete/${id}`, JSON.stringify(notes), { headers: this.getHeaders() });
  }
}
