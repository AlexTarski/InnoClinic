import {Component, inject, OnInit, ViewEncapsulation} from '@angular/core';
import {CommonModule, DatePipe} from '@angular/common';
import {DoctorService} from "../../data/services/doctor.service";
import {Doctor} from "../../data/interfaces/doctor.interface";
import {SvgIconComponent} from "../../components/svg-icon/svg-icon.component";
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";
import {DoctorStatusService} from "../../data/services/doctorStatus.service";
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';

@Component({
  selector: 'app-doctors',
  standalone: true,
	imports: [CommonModule, SvgIconComponent, MatFormField, MatSelect, MatOption, MatLabel],
	providers: [DatePipe],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorsComponent implements OnInit {
  doctorService = inject(DoctorService);
	officeService = inject(OfficeService);
	doctorStatusService = inject(DoctorStatusService);
	datePipe = inject(DatePipe);
  doctors: Doctor[] = [];
	officeMap: Record<string, Office> = {};

  constructor(){}

	ngOnInit() {
		this.doctorService.getDoctors()
				.subscribe(doctor => {
					this.doctors = doctor;

					this.officeService.getOffices().subscribe(offices => {
						this.officeMap = offices.reduce((map, office) => {
							map[office.id!] = office;
							return map;
						}, {} as Record<string, Office>);
					});
				});
	}

	formatDoctorBirthDate(birthDate: Date) {
		return this.datePipe.transform(birthDate, 'shortDate');
	}

	protected viewDoctorInfo(doctor: Doctor) {

	}

	protected createDoctor() {

	}

	protected getOfficeAddress(officeId: string) {
		const office = this.officeMap[officeId];
		return office ? this.formatOfficeAddressToString(office) : '';
	}

	private formatOfficeAddressToString(office: Office) {
		return `${office.address.city}, ${office.address.street}, ${office.address.houseNumber}, ${office.address.officeNumber}`;
	}
}