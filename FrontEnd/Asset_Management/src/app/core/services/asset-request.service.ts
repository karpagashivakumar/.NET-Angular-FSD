import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AssetRequest } from '../models/asset-request.model';

@Injectable({
  providedIn: 'root'
})
export class AssetRequestService {
  private baseUrl = 'http://localhost:5116/api/v1/AssetRequest';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('jwtToken');
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  createRequest(dto: { assetCategory: string; requestDescription: string }): Observable<any> {
    return this.http.post(`${this.baseUrl}/with-dto`, dto, {
      headers: this.getHeaders()
    });
  }

  getMyRequests(): Observable<AssetRequest[]> {
  return this.http.get<AssetRequest[]>(`${this.baseUrl}/my`, {
    headers: this.getHeaders()
  });
}
}
