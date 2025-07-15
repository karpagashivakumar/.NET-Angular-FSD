// src/app/features/user/asset-allocation/asset-allocation.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AssetAllocation } from '../models/allocation.model'; // You should define this

@Injectable({ providedIn: 'root' })
export class AssetAllocationService {
  private baseUrl = 'http://localhost:5116/api/v1/AssetAllocation';

  constructor(private http: HttpClient) {}

  getMyAllocations(): Observable<AssetAllocation[]> {
    return this.http.get<AssetAllocation[]>(`${this.baseUrl}/my`);
  }

  returnAsset(id: number, remarks: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/return/${id}`, { remarks });
  }
}
