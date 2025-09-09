import {Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {DoctorsComponent} from './pages/doctors/doctors.component';
import {PatientsComponent} from "./pages/patients/patients.component";
import {LoginComponent} from "./components/login-page/login-page.component";
import {ServerOfflineComponent} from "./pages/server-offline/server-offline.component";
import {canActivateAuth} from "./data/auth-guard";
import {LoginSuccessComponent} from "./pages/login-success/login-success.component";
import {OfficesComponent} from "./pages/offices/offices.component";

export const routes: Routes = [
	{path: 'login', component: LoginComponent},
	{path: 'error', component: ServerOfflineComponent},
	{path: 'login-success', component: LoginSuccessComponent},
	{
		path: '', component: HomeComponent,
		children: [
			{path: 'patients', component: PatientsComponent},
			{path: 'doctors', component: DoctorsComponent},
			{path: 'offices', component: OfficesComponent},
			{path: 'specializations', component: DoctorsComponent,}, // Placeholder
			{path: 'profile', component: DoctorsComponent,}, // Placeholder
		],
		canActivate: [canActivateAuth]
	},
	{path: '**', redirectTo: ''}
];