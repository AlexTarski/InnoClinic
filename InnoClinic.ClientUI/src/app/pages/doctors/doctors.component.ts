import {Component, inject, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DoctorCard} from "../../components/doctor-card/doctor-card";
import {DoctorService} from "../../data/services/doctor-service";
import {Doctor} from "../../data/interfaces/doctors.interface";
import {SvgIconComponent} from "../../components/svg-icon/svg-icon.component";
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';
import {ServicesService} from "../../data/services/services.service";
import {toSignal} from "@angular/core/rxjs-interop";

@Component({
  selector: 'app-doctors',
  standalone: true,
	imports: [CommonModule, DoctorCard, SvgIconComponent, MatFormField, MatLabel, MatOption, MatSelect],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorsComponent {
  doctorService = inject(DoctorService);
	servicesService = inject(ServicesService);
	protected specializations = toSignal(this.servicesService.getSpecializations(), { initialValue: [] });
  doctors = toSignal(this.doctorService.getDoctors(), { initialValue: [] });
	currentYear: number = this.getCurrentYear();

  constructor(){
  }

	getCurrentYear(){
		return new Date().getFullYear();
	}
}