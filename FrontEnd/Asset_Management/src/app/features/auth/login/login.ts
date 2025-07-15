import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router'; 
import { AuthService } from '../../../core/auth'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';
  showPassword = false;

  constructor(private auth: AuthService, private router: Router) {}

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  login(): void {
  const credentials = {
    username: this.username,
    password: this.password
  };

  this.auth.login(credentials).subscribe({
    next: (res: any) => {
      localStorage.setItem('token', res.token);

      const decoded = this.parseJwt(res.token);
      if (decoded) {
        const user = {
          username: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
          role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
          firstName: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
        };
        localStorage.setItem('user', JSON.stringify(user));
      }

      setTimeout(() => {
    this.router.navigate(['/dashboard']);
  }, 100);
    },
    error: () => {
      this.errorMessage = 'Invalid username or password';
    }
  });
}

private parseJwt(token: string): any {
  try {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  } catch (e) {
    return null;
  }
}
}
