import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Asset } from '../models/asset.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AssetService {
  private baseUrl = 'http://localhost:5116/api/v1/Asset';

  constructor(private http: HttpClient) {}

  getAllAssets(): Observable<Asset[]> {
    return this.http.get<Asset[]>(this.baseUrl);
  }

  getAssetById(id: number): Observable<Asset> {
    return this.http.get<Asset>(`${this.baseUrl}/${id}`);
  }
}
