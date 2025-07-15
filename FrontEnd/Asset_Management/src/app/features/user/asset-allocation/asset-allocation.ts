import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AssetAllocation } from '../../../core/models/allocation.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-asset-allocation',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './asset-allocation.html',
  styleUrls: ['./asset-allocation.css']
})
export class AssetAllocationComponent implements OnInit {
  allocations: AssetAllocation[] = [];
  filteredAllocations: AssetAllocation[] = [];
  apiUrl = 'http://localhost:5116/api/v1/AssetAllocation/my';
  filterStatus: string = 'All';
  returnRemarks: string = '';
  selectedId: number | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchAllocations();
  }

  fetchAllocations(): void {
    this.http.get<AssetAllocation[]>(this.apiUrl).subscribe({
      next: (res) => {
        this.allocations = res;
        this.applyFilter();
      },
      error: (err) => console.error('Failed to fetch allocations', err)
    });
  }

  applyFilter(): void {
    if (this.filterStatus === 'All') {
      this.filteredAllocations = this.allocations;
    } else {
      this.filteredAllocations = this.allocations.filter(a => a.allocationStatus === this.filterStatus);
    }
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  prepareReturn(id: number): void {
    this.selectedId = id;
    this.returnRemarks = '';
  }

  returnAsset(): void {
    if (!this.selectedId) return;

    const url = `http://localhost:5116/api/v1/AssetAllocation/return/${this.selectedId}`;
    const body = { remarks: this.returnRemarks };

    this.http.put(url, body).subscribe({
      next: () => {
        this.selectedId = null;
        this.returnRemarks = '';
        this.fetchAllocations(); // refresh
        alert('Asset returned successfully.');
      },
      error: () => {
        alert('Failed to return asset.');
      }
    });
  }
}
