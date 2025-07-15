import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AssetService } from '../../../core/services/asset.service';
import { Asset } from '../../../core/models/asset.model';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-asset',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './asset.html',
  styleUrls: ['./asset.css']
})
export class AssetComponent implements OnInit {
  assets: any[] = [];
  searchTerm: string = '';
  selectedCategory: string = '';
  categories: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('http://localhost:5116/api/v1/Asset').subscribe(res => {
      this.assets = res;
      this.categories = [...new Set(res.map(asset => asset.assetCategory))];
    });
  }

  filteredAssets(): any[] {
    return this.assets.filter(asset =>
      (this.selectedCategory === '' || asset.assetCategory === this.selectedCategory) &&
      (this.searchTerm === '' ||
        asset.assetName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        asset.assetNo.toLowerCase().includes(this.searchTerm.toLowerCase()))
    );
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedCategory = '';
  }

  getAssetIcon(category: string): string {
    switch (category.toLowerCase()) {
      case 'laptop':
        return 'bi bi-laptop';
      case 'monitor':
        return 'bi bi-display';
      case 'printer':
        return 'bi bi-printer-fill';
      case 'keyboard':
        return 'bi bi-keyboard';
      case 'mouse':
        return 'bi bi-mouse2-fill';
      case 'mobile':
        return 'bi bi-phone';
      case 'server':
        return 'bi bi-hdd-stack';
      default:
        return 'bi bi-box';
    }
  }
}
