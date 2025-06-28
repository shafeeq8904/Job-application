import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { NgChartsModule } from 'ng2-charts';
import type { ChartType ,ChartData } from 'chart.js';
import { ApplicationService } from '../../services/application.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, NgChartsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent implements OnInit {
  totalApplications = 0;
  statusCounts: { [key: string]: number } = {};
  applicationsPerUser: any[] = [];

  chartLabels: string[] = [];
  chartType: ChartType = 'pie';
  chartData: ChartData<'pie', number[], string | string[]> = {
  labels: [],
  datasets: [{ data: [] }]
};

  constructor(private http: HttpClient,private appService: ApplicationService) {}

  ngOnInit(): void {
    this.appService.getAdminSummary().subscribe({
      next: (res) => {
        const data = res.data;
        this.totalApplications = data.totalApplications;
        this.statusCounts = data.statusCounts;
        this.applicationsPerUser = data.applicationsPerUser;

        this.chartData = {
          labels: Object.keys(this.statusCounts),
          datasets: [{ data: Object.values(this.statusCounts) }]
        };
      }
    });
  }
}
