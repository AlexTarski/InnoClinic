import {Component, computed, effect, inject, Input, OnInit, signal, ViewEncapsulation} from '@angular/core';
import {Doctor} from "../../data/interfaces/doctors.interface";
import {Address} from "../../data/interfaces/address.interface";
import {OfficeService} from "../../data/services/office.service";
import {FileService} from "../../data/services/file.service";
import {SafeUrl} from "@angular/platform-browser";
import {Specialization} from "../../data/interfaces/services/specialization.interface";

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
	@Input() specializations!: Specialization[];
	specialization = computed(() => this.getSpecialization(this.doctor.specializationId))
	photoUrl = signal<SafeUrl>('');
	isReady = signal(false);
	officeService = inject(OfficeService);
	fileService = inject(FileService);
	officeAddress!: Address;

	get experience(): number {
		return this.doctor && this.currentYear
				? this.currentYear - this.doctor.careerStartYear + 1
				: 0;
	}

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

		this.photoUrl.set(await this.fileService.getEmployeePhoto(this.doctor.accountId));
	}

	getSpecialization(specializationId: string) {
		const specialization = this.specializations
				.filter(spec => spec.id === specializationId)
				.map(spec => spec.name);

		return specialization[0];
	}
}