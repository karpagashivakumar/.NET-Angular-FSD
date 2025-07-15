import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ServiceRequest, ServiceRequestDto } from '../../../core/models/service-request.model';

@Component({
  selector: 'app-service-request',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './service-request.html',
  styleUrls: ['./service-request.css']
})
export class ServiceRequestComponent implements OnInit {
  requests: ServiceRequest[] = [];
  newRequest: ServiceRequestDto = { assetId: 0, assetNo: '', issueType: '', description: '' };
  apiUrl = 'http://localhost:5116/api/v1/ServiceRequest/my-requests';
  postUrl = 'http://localhost:5116/api/v1/ServiceRequest/with-dto';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.http.get<ServiceRequest[]>(this.apiUrl).subscribe({
      next: (res) => (this.requests = res),
      error: (err) => console.error('Error loading requests', err)
    });
  }

  submitRequest(): void {
    this.http.post(this.postUrl, this.newRequest).subscribe({
      next: () => {
        alert('Request submitted successfully!');
        this.newRequest = { assetId: 0, assetNo: '', issueType: '', description: '' };
        this.loadRequests();
      },
      error: () => alert('Submission failed!')
    });
  }
}