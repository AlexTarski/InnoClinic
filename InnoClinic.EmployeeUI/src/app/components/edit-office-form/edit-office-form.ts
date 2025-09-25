import {Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatDialog} from "@angular/material/dialog";
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";
import {ConfirmDialog} from "../confirm-dialog/confirm-dialog";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";
import {DoctorService} from "../../data/services/doctor.service";
import {photoTypeValidator} from "../../data/directives/photo-type-validator.directive";
import {FileService} from "../../data/services/file.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-edit-office-form',
	imports: [
		FormsModule,
		PhonePlusValidatorDirective,
		ReactiveFormsModule
	],
	standalone: true,
  templateUrl: `./edit-office-form.html`,
  styleUrl: `./edit-office-form.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class EditOfficeForm implements OnInit {
	@Input() office: Office = {
		id: undefined,
		address: { city: '', street: '', houseNumber: '', officeNumber: '' },
		registryPhoneNumber: '+',
		isActive: true,
		photoId: undefined
	};
	@Output() onChanged = new EventEmitter<{ edited: boolean; office: Office | undefined }>();
	isDisabled: boolean = false
	form: FormGroup = new FormGroup({
		city: new FormControl("", [Validators.required, Validators.pattern(/\S+/)]),
		street: new FormControl("", [Validators.required, Validators.pattern(/\S+/)]),
		houseNumber: new FormControl("", [Validators.required, Validators.pattern(/\S+/)]),
		officeNumber: new FormControl("", [Validators.required, Validators.pattern(/\S+/)]),
		status: new FormControl(true, [Validators.required]),
		registryPhoneNumber: new FormControl("+", [Validators.required]),
		photo: new FormControl(null, [photoTypeValidator])
	});

	constructor(private dialog: MatDialog,
							private officeService: OfficeService,
							private doctorService: DoctorService,
							private fileService: FileService) {
	}

	ngOnInit() {
		if (this.office) {
			this.form.patchValue({
				city: this.office.address.city,
				street: this.office.address.street,
				houseNumber: this.office.address.houseNumber,
				officeNumber: this.office.address.officeNumber,
				status: this.office.isActive,
				registryPhoneNumber: this.office.registryPhoneNumber
						? this.office.registryPhoneNumber.replace(/[^\d+]/g, "")
						: "+"
			});
		}
	}

	async onSubmit(){
		this.isDisabled = true;
		const formValue = this.form.value;

		const office: Office = {
			id: this.office.id,
			address: {
				city: formValue.city,
				street: formValue.street,
				houseNumber: formValue.houseNumber,
				officeNumber: formValue.officeNumber
			},
			registryPhoneNumber: formValue.registryPhoneNumber,
			isActive: formValue.status,
			photoId: this.office.photoId,
		};

		const photoFile: File | null = this.form.get('photo')?.value;

		if (photoFile)
		{
			// try to update photo by current office.photo id; if not successful - addPhoto instead
			const response = await firstValueFrom(
					this.fileService.updatePhoto(office.photoId, photoFile));

			if (response.status === 404)
			{
				office.photoId = await firstValueFrom(this.fileService.addPhoto(photoFile));
			}
		}

		await firstValueFrom(this.officeService.updateOffice(office, office.id));

		if (!office.isActive) {
			this.doctorService.deactivateDoctorsByOfficeId(office.id)
		}

		this.change(true, office);
		this.isDisabled = false;
	}

	onCancel() {
		const ref = this.dialog.open(ConfirmDialog, {
			disableClose: true,
		});

		ref.afterClosed().subscribe(result => {
			if (result === true) {
				this.change(false, undefined);
			}
		});
	}

	change(edited: boolean, office: Office | undefined) {
		this.onChanged.emit({edited: edited, office: office});
	}

	onFileSelected(event: Event) {
		const input = event.target as HTMLInputElement;
		const file = input.files && input.files.length ? input.files[0] : null;

		this.form.get('photo')?.setValue(file);
		this.form.get('photo')?.markAsDirty();
		this.form.get('photo')?.updateValueAndValidity();
	}
}