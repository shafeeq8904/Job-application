import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  name = '';
  role = 'User';
  password = '';
  error = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  login() {
    if (!this.name || !this.password || !this.role) {
      this.error = 'All fields are required';
      return;
    }

    this.auth.login(this.name, this.role, this.password).subscribe({
      next: (res) => {
        this.auth.saveSession(res.data);
        const redirect = this.role === 'Admin' ? '/admin/dashboard' : '/user/dashboard';
        this.router.navigate([redirect]);
      },
      error: () => {
        this.error = 'Invalid name, role, or password';
      }
    });
  }
}
