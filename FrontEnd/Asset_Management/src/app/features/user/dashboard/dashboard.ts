import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit {
  username: string = 'Employee';

  dashboardCards = [
    {
    title: 'All Assets',
    description: 'Browse available company assets.',
    icon: 'bi bi-laptop text-secondary',
    route: '/asset',
  },
    {
      title: 'Asset Requests',
      description: 'Request and track your assets.',
      icon: 'bi bi-box-seam text-primary',
      route: '/asset-request',
    },
    {
      title: 'Asset Audits',
      description: 'View audit reports of your assets.',
      icon: 'bi bi-clipboard-check text-success',
      route: '/asset-audit',
    },
    {
      title: 'Service Requests',
      description: 'Raise service/repair tickets.',
      icon: 'bi bi-tools text-warning',
      route: '/service-request',
    },
    {
      title: 'Asset Allocations',
      description: 'See your allocated assets.',
      icon: 'bi bi-hdd-network text-info',
      route: '/asset-allocation',
    },
  ];

  ngOnInit(): void {
    const user = localStorage.getItem('user');
    if (user && user !== 'undefined') {
      try {
        const parsed = JSON.parse(user);
        this.username = parsed.firstName || parsed.username || 'Employee';
      } catch (e) {
        console.error('Invalid user object in localStorage', e);
      }
    }
  }
}
