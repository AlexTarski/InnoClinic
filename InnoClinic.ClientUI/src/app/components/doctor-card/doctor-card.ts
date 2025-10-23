import {Component, inject, Input, OnInit, ViewEncapsulation} from '@angular/core';
import {Doctor} from "../../data/interfaces/doctors.interface";
import {Address} from "../../data/interfaces/address.interface";
import {OfficeService} from "../../data/services/office.service";
import {FileService} from "../../data/services/file.service";

@Component({
  selector: 'app-doctor-card',
  imports: [
  ],
  templateUrl: `./doctor-card.component.html`,
  styleUrl: `./doctor-card.component.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorCard implements OnInit {
  @Input() doctor!: Doctor;
	@Input() currentYear!: number;
	officeService = inject(OfficeService);
	fileService = inject(FileService);
	officeAddress!: Address;

	ngOnInit(): void {
		if (this.doctor?.officeId) {
			this.officeService.getOffice(this.doctor.officeId).subscribe(office => {
				this.officeAddress = office.address;
			});
		}
	}

	get experience(): number {
		return this.doctor && this.currentYear
				? this.currentYear - this.doctor.careerStartYear + 1
				: 0;
	}
}