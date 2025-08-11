import {Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {DoctorsComponent} from './pages/doctors/doctors.component';
import {PatientsComponent} from "./pages/patients/patients.component";
import {LoginPageComponent} from "./components/login-page/login-page.component";

export const routes: Routes = [
	{
		path: '', component: HomeComponent, children: [
			{path: 'patients', component: PatientsComponent},
			{path: 'doctors', component: DoctorsComponent},
			{path: 'specializations', component: DoctorsComponent,}, // Placeholder
			{path: 'profile', component: DoctorsComponent,}, // Placeholder
		]
	},
	{path: 'login', component: LoginPageComponent},
	{path: '**', redirectTo: ''}
];