import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicationService } from '../../services/application.service';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-all-applications',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './admin-all-applications.component.html',
  styleUrl: './admin-all-applications.component.css'
})
export class AdminAllApplicationsComponent implements OnInit {
  applications: any[] = [];
  statusOptions: string[] = ['Applied', 'Interview Scheduled', 'Offered', 'Rejected'];
  companyOptions: string[] = ['Presidio', 'Google', 'Amazon', 'Microsoft', 'Apple', 'accenture'];


  noDataMessage = '';
  selectedStatus: string = '';
  selectedCompany: string = '';

  constructor(private appService: ApplicationService) {}

  ngOnInit(): void {

    this.loadApplications();               // ✅ For filtered application list
  }

  // ✅ Load full dataset (unfiltered) to extract dropdown values
  loadAllApplicationsForDropdown() {
    const role = localStorage.getItem('role') ?? '';
    const userId = Number(localStorage.getItem('userId')) || 0;
    


    this.appService.getAllApplications(userId, role).subscribe({
      next: (res) => {
        const allApps = res.data || [];
        this.statusOptions = [...new Set(allApps.map(app => app.status))];
        this.companyOptions = [...new Set(allApps.map(app => app.companyName))];
        console.log("All dropdown options:", this.statusOptions, this.companyOptions);
      },
      error: () => {
        this.statusOptions = [];
        this.companyOptions = [];
      }
    });
  }

  // ✅ Load filtered application list
  loadApplications() {
    this.appService.getFilteredApplications(this.selectedStatus, this.selectedCompany).subscribe({
      next: (res) => {
        this.applications = res.data || [];
        this.noDataMessage = res.message || '';
      },
      error: () => {
        this.applications = [];
        this.noDataMessage = 'Failed to load applications.';
      }
    });
  }

  get filtersActive(): boolean {
    return !!this.selectedStatus || !!this.selectedCompany;
  }

  applyFilters() {
    this.loadApplications();
  }

  clearFilters() {
    this.selectedStatus = '';
    this.selectedCompany = '';
    this.loadApplications();
  }
}
