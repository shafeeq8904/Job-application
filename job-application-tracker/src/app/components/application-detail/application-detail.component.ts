import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApplicationService } from '../../services/application.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-application-detail',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './application-detail.component.html',
})
export class ApplicationDetailComponent implements OnInit {
  app: any;
  logs: any[] = [];
  isAdmin = false;
  editing = false;
  updatedStatus = '';
  updatedNotes = '';

  constructor(
    private route: ActivatedRoute,
    private appService: ApplicationService,
    private auth: AuthService
  ) {}
  
  ngOnInit(): void {
    this.isAdmin = this.auth.isAdmin();
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const userId = this.auth.userId;
    const role = this.auth.role;

  this.appService.getApplicationById(id, userId, role).subscribe({
    next: (res: any) => this.app = res.data
  });

  this.appService.getStatusLogs(id).subscribe({
    next: (res: any) => this.logs = res.data
  });
}
toggleEdit() {
  this.updatedStatus = this.app.status;
  this.updatedNotes = this.app.notes;
  this.editing = true;
}

saveChanges() {
  this.appService.updateApplication(this.app.id, {
    ...this.app,
    status: this.updatedStatus,
    notes: this.updatedNotes
  }).subscribe({
    next: () => {
      this.app.status = this.updatedStatus;
      this.app.notes = this.updatedNotes;
      this.editing = false;
      this.ngOnInit(); // Refresh logs
    },
    error: () => alert('Update failed')
  });
}

deleteApplication() {
  if (!confirm('Are you sure you want to delete this application?')) return;

  this.appService.deleteApplication(this.app.id, this.auth.userId, this.auth.role).subscribe({
    next: () => {
      alert('Application deleted');
      window.location.href = this.auth.isAdmin() ? '/admin/applications' : '/user/dashboard';
    },
    error: () => alert('Delete failed')
  });
}
}
