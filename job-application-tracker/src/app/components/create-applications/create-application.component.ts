import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApplicationService } from '../../services/application.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-application',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-application.component.html'
})
export class CreateApplicationComponent {
  companyOptions: string[] = ['Presidio', 'Google', 'Amazon', 'Microsoft', 'Apple'];
  statusOptions: string[] = ['Applied', 'Interview Scheduled', 'Offered', 'Rejected'];

  application = {
  jobTitle: '',
  companyName: '',
  location: '',
  applicationDate: '',
  status: 'Applied',
  notes: ''
};

  error = '';

  constructor(
    private appService: ApplicationService,
    private router: Router
  ) {}


ngOnInit(): void {
  const today = new Date().toISOString().split('T')[0]; 
  this.application.applicationDate = today;
}

  submitApplication() {
    const userId = Number(localStorage.getItem('userId'));
    const payload = { ...this.application, userId };

    this.appService.createApplication(payload).subscribe({
      next: () => {
        this.router.navigate(['/user/dashboard']);
      },
      error: () => {
        this.error = 'Failed to create application';
      }
    });
    console.log('Application submitted:', payload);
  }
}
