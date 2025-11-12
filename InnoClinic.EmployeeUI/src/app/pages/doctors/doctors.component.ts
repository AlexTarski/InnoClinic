import {Component, inject, ViewEncapsulation} from '@angular/core';
import {CommonModule, DatePipe} from '@angular/common';
import {DoctorService} from "../../data/services/doctor.service";
import {Doctor} from "../../data/interfaces/doctor.interface";
import {SvgIconComponent} from "../../components/svg-icon/svg-icon.component";

@Component({
  selector: 'app-doctors',
  standalone: true,
	imports: [CommonModule, SvgIconComponent],
	providers: [DatePipe],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorsComponent {
  doctorService = inject(DoctorService);
	datePipe = inject(DatePipe);
  doctors: Doctor[] = [];

  constructor(){
    this.doctorService.getDoctors()
        .subscribe(doctor => {
          this.doctors = doctor
        });
  }

	formatDoctorBirthDate(birthDate: Date) {
		return this.datePipe.transform(birthDate, 'shortDate');
	}

	protected viewDoctorInfo(doctor: Doctor) {

	}

	protected createDoctor() {

	}
}