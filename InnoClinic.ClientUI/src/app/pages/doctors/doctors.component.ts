import {Component, inject, ViewEncapsulation} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DoctorCard} from "../../components/doctor-card/doctor-card";
import {DoctorService} from "../../data/services/doctor-service";
import {Doctor} from "../../data/interfaces/doctors.interface";

@Component({
  selector: 'app-doctors',
  standalone: true,
	imports: [CommonModule, DoctorCard],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorsComponent {
  doctorService = inject(DoctorService);
  doctors: Doctor[] = [];
	currentYear: number = this.getCurrentYear();

  constructor(){
    this.doctorService.getDoctors()
        .subscribe(doctor => {
          this.doctors = doctor
        });
  }

	getCurrentYear(){
		return new Date().getFullYear();
	}
}