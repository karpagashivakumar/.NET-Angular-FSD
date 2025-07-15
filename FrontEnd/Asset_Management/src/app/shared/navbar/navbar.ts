import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css'],
  imports: [RouterModule] 
})
export class NavbarComponent implements OnInit {
  username: string = 'Employee';

  constructor(private router: Router) {}

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

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
