import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  private baseUrl = 'http://localhost:5095/api/user';

  // ------------------------------
  // Login
  // ------------------------------
  login(name: string, role: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}/login`, { name, role, password });
  }

  // ------------------------------
  // Register
  // ------------------------------
  register(name: string, role: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}/create`, { name, role, password });
  }

  // ------------------------------
  // Session Management
  // ------------------------------
  saveSession(user: { id: number, name: string, role: string }) {
    localStorage.setItem('userId', user.id.toString());
    localStorage.setItem('userName', user.name);
    localStorage.setItem('role', user.role);
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  // ------------------------------
  // Accessors
  // ------------------------------
  get userId(): number {
    return Number(localStorage.getItem('userId'));
  }

  get userName(): string {
    return localStorage.getItem('userName') || '';
  }

  get role(): string {
    return localStorage.getItem('role') || '';
  }

  isAdmin(): boolean {
    return this.role === 'Admin';
  }

  isUser(): boolean {
    return this.role === 'User';
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('userId');
  }
}
