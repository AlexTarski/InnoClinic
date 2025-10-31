import { Routes } from '@angular/router';
import { HomeComponent } from "./pages/home/home.component";
import { DoctorsComponent } from './pages/doctors/doctors.component';
import {WelcomeCard} from "./components/welcome-card/welcome-card";

export const routes: Routes = [
	{
		path: '', component: HomeComponent,
		children: [
			{path: '', component: WelcomeCard},
			{ path: 'doctors', component: DoctorsComponent },
			{ path: 'specializations', component: WelcomeCard }, // Placeholder
			{ path: 'medical-results', component: WelcomeCard }, // Placeholder
			{ path: 'profile', component: WelcomeCard }, // Placeholder
		]
	},
  { path: '**', redirectTo: '' }
];