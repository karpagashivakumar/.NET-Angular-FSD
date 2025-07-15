import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AssetRequestService } from '../../../core/services/asset-request.service';
import { AssetRequest } from '../../../core/models/asset-request.model';
import { ToastrService } from 'ngx-toastr';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-asset-request',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './asset-request.html',
  styleUrls: ['./asset-request.css']
})
export class AssetRequestComponent implements OnInit {
  requests: AssetRequest[] = [];
  newRequest = { assetCategory: '', requestDescription: '' };

  constructor(
    private assetRequestService: AssetRequestService,
    private toastr: ToastrService
  ) {}

  getBadgeClass(status: string | undefined): string {
  switch ((status || '').toLowerCase()) {
    case 'approved':
      return 'success';
    case 'rejected':
      return 'danger';
    case 'pending':
    default:
      return 'warning';
  }
}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
  this.assetRequestService.getMyRequests().subscribe({
    next: (data) => (this.requests = data),
    error: () => this.toastr.error('Failed to load your requests')
  });
}

  submitRequest(): void {
    this.assetRequestService.createRequest(this.newRequest).subscribe({
      next: () => {
        this.toastr.success('Request submitted');
        this.newRequest = { assetCategory: '', requestDescription: '' };
        this.loadRequests();
      },
      error: () => this.toastr.error('Failed to submit request')
    });
  }
}
