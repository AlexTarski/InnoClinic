import {Component, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";
import {ConfirmDialog} from "../confirm-dialog/confirm-dialog";
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";
import {MatDialog} from "@angular/material/dialog";
import {DialogRef} from "@angular/cdk/dialog";
import {Router} from "@angular/router";
import {photoTypeValidator} from "../../data/directives/photo-type-validator.directive";
import {firstValueFrom} from "rxjs";
import {FileService} from "../../data/services/file.service";

@Component({
  selector: 'app-create-office-form',
	imports: [
		FormsModule, ReactiveFormsModule, PhonePlusValidatorDirective
	],
  templateUrl: `./create-office-form.html`,
  styleUrl: `./create-office-form.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class CreateOfficeForm {
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
							private dialogRef: DialogRef<CreateOfficeForm>,
							private officeService: OfficeService,
							private fileService: FileService,
							private router: Router) {
	}

	async onSubmit(){
		this.isDisabled = true;
		const formValue = this.form.value;

		const office: Office = {
			id: undefined,
			address: {
				city: formValue.city,
				street: formValue.street,
				houseNumber: formValue.houseNumber,
				officeNumber: formValue.officeNumber
			},
			registryPhoneNumber: formValue.registryPhoneNumber,
			isActive: formValue.status,
			photoId: undefined,
		};

		const photoFile: File | null = this.form.get('photo')?.value;

		if (photoFile)
		{
			office.photoId = await firstValueFrom(this.fileService.addOfficePhoto(photoFile));
		}

		await firstValueFrom(this.officeService.addOffice(office));
		const currentUrl = this.router.url;
		this.isDisabled = false;
		this.dialogRef.close();
		this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
			this.router.navigate([currentUrl])
		});
	}

	onCancel() {
		const ref = this.dialog.open(ConfirmDialog, {
		disableClose: true,
		});

		ref.afterClosed().subscribe(result => {
			if (result === true) {
				this.dialogRef.close();
			}
		});
	}

	onFileSelected(event: Event) {
		const input = event.target as HTMLInputElement;
		const file = input.files && input.files.length ? input.files[0] : null;

		this.form.get('photo')?.setValue(file);
		this.form.get('photo')?.markAsDirty();
		this.form.get('photo')?.updateValueAndValidity();
	}
}