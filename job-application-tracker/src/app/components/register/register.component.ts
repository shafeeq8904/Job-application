import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterEvent, RouterLink } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  name = '';
  role = 'User';
  password = '';
  confirmPassword = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    if (!this.name || !this.password || !this.confirmPassword || !this.role) {
      this.error = 'All fields are required';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    this.auth.register(this.name, this.role, this.password).subscribe({
      next: (res) => {
        // Optional: auto-login
        this.auth.saveSession(res.data);
        this.router.navigate(['/login']); // or redirect to dashboard
      },
      error: () => {
        this.error = 'Registration failed';
      }
    });
  }
}
