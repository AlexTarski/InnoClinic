import { Routes } from '@angular/router';
import { HomeComponent } from "./pages/home/home.component";
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import {PatientsComponent} from "./pages/patients/patients.component";

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'doctors', component: DoctorsComponent },
  { path: 'specializations', component: HomeComponent }, // Placeholder
  { path: 'patients', component: PatientsComponent },
  { path: 'profile', component: HomeComponent }, // Placeholder
  { path: '**', redirectTo: '' }
];
