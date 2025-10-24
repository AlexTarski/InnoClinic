import {Component, effect, inject, Input, OnInit, signal, ViewEncapsulation} from '@angular/core';
import {Doctor} from "../../data/interfaces/doctors.interface";
import {Address} from "../../data/interfaces/address.interface";
import {OfficeService} from "../../data/services/office.service";
import {FileService} from "../../data/services/file.service";
import {SafeUrl} from "@angular/platform-browser";

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
	photoUrl = signal<SafeUrl>('');
	isReady = signal(false);
	officeService = inject(OfficeService);
	fileService = inject(FileService);
	officeAddress!: Address;

	constructor() {
		effect(() => {
			const url = this.photoUrl();
			if (url !== '') {
				this.isReady.set(true);
			}
		});
	}

	async ngOnInit(): Promise<void> {
		if (this.doctor?.officeId) {
			this.officeService.getOffice(this.doctor.officeId).subscribe(office => {
				this.officeAddress = office.address;
			});
		}

		this.photoUrl.set(await this.fileService.getDoctorPhoto(this.doctor.accountId));
	}

	get experience(): number {
		return this.doctor && this.currentYear
				? this.currentYear - this.doctor.careerStartYear + 1
				: 0;
	}
}