import {Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {DoctorsComponent} from './pages/doctors/doctors.component';
import {PatientsComponent} from "./pages/patients/patients.component";
import {LoginComponent} from "./components/login-page/login-page.component";
import {ServerOfflineComponent} from "./pages/server-offline/server-offline.component";
import {AuthGuard} from "./data/auth-guard";

export const routes: Routes = [
	{
		path: '', component: HomeComponent, canActivate: [AuthGuard],
		children: [
			{path: 'patients', component: PatientsComponent},
			{path: 'doctors', component: DoctorsComponent},
			{path: 'specializations', component: DoctorsComponent,}, // Placeholder
			{path: 'profile', component: DoctorsComponent,}, // Placeholder
		]
	},
	{path: 'login', component: LoginComponent},
	{path: 'error', component: ServerOfflineComponent},
	{path: '**', redirectTo: ''}
];