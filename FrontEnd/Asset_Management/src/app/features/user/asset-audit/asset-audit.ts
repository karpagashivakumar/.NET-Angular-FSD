import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AssetAuditService } from '../../../core/services/asset-audit.service';
import { AssetAudit } from '../../../core/models/asset-audit.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-asset-audit',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './asset-audit.html',
  styleUrls: ['./asset-audit.css']
})
export class AssetAuditComponent implements OnInit {
  audits: AssetAudit[] = [];
  //newAudit: { assetId: number; employeeId: number; auditNotes?: string } = { assetId: 0, employeeId: 0 };

  constructor(private auditService: AssetAuditService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadAudits();
  }

  loadAudits(): void {
  this.auditService.getMyAudits().subscribe({
    next: (data) => (this.audits = data),
    error: () => this.toastr.error('Failed to load audits')
  });
}

  // submitAudit(): void {
  //   this.auditService.createAudit(this.newAudit).subscribe({
  //     next: () => {
  //       this.toastr.success('Audit created');
  //       this.newAudit = { assetId: 0, employeeId: 0 };
  //       this.loadAudits();
  //     },
  //     error: () => this.toastr.error('Failed to create audit')
  //   });
  // }

  completeAudit(id: number): void {
    const notes = prompt('Enter completion notes');
    if (notes) {
      this.auditService.completeAudit(id, notes).subscribe({
        next: () => {
          this.toastr.success('Audit completed');
          this.loadAudits();
        },
        error: () => this.toastr.error('Completion failed')
      });
    }
  }
}