import { Routes } from '@angular/router';
import { LoginComponent } from './components/login';
import { RegisterComponent } from './components/register/register.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { AdminAllApplicationsComponent } from './components/admin-all-applications/admin-all-applications.component';
import { ApplicationDetailComponent } from './components/application-detail/application-detail.component';
import { CreateApplicationComponent } from './components/create-applications/create-application.component';

export const routes: Routes = [
    {path : 'login' , component:LoginComponent},
    {path: 'register',component:RegisterComponent},
    {path: 'user/dashboard',component:UserDashboardComponent},
    {path: 'admin/dashboard',component:AdminDashboardComponent}, 
    {path: 'user/createapplication',component:CreateApplicationComponent},
    {path: 'admin/applications',component:AdminAllApplicationsComponent},
    {path: 'applications/:id',component:ApplicationDetailComponent},
    { path: '', redirectTo: 'login', pathMatch: 'full' }
];
