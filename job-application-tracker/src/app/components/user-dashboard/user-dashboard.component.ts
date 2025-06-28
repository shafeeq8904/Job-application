import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { ApplicationService } from '../../services/application.service';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.css'
})
export class UserDashboardComponent implements OnInit {
  userName = '';
  totalApplications = 0;
  statusCounts: { [key: string]: number } = {};
  applications: any[] = [];

  constructor(private http: HttpClient, public auth: AuthService,private appService: ApplicationService,) {}

  ngOnInit(): void {
    
    this.userName = this.auth.userName;
    const userId = this.auth.userId;
    const role = this.auth.role;

    this.appService.getApplications(userId, role).subscribe({
      next: (res) => {
        this.applications = res.data;
        this.totalApplications = this.applications.length;

        // Count by status
        this.statusCounts = this.applications.reduce((acc, app) => {
          acc[app.status] = (acc[app.status] || 0) + 1;
          return acc;
        }, {} as { [key: string]: number });
      },
      error: () => {
        this.totalApplications = 0;
        this.statusCounts = {};
      }
    });
  }

  statusKeys(): string[] {
  return Object.keys(this.statusCounts);
}

}
