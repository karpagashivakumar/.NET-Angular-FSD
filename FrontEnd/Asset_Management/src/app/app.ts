import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, NavbarComponent, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent implements OnInit {
  username: string = 'Employee';

  constructor(private router: Router) {}

  isLoginPage(): boolean {
    return this.router.url.includes('/login');
  }

  isDashboardPage(): boolean {
    return this.router.url === '/dashboard';
  }

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
