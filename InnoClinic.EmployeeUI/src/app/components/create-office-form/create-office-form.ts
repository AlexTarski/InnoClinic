import {Component, EventEmitter, Output, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";
import {ConfirmDialog} from "../confirm-dialog/confirm-dialog";
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";
import {MatDialog} from "@angular/material/dialog";
import {DialogRef} from "@angular/cdk/dialog";
import {Router} from "@angular/router";

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
		city: new FormControl("", [Validators.required]),
		street: new FormControl("", [Validators.required]),
		houseNumber: new FormControl("", [Validators.required]),
		officeNumber: new FormControl("", [Validators.required]),
		status: new FormControl(true, [Validators.required]),
		registryPhoneNumber: new FormControl("+", [Validators.required]),
		photo: new FormControl("")
	});

	constructor(private dialog: MatDialog,
							private dialogRef: DialogRef<CreateOfficeForm>,
							private officeService: OfficeService,
							private router: Router) {
	}

	onSubmit(){
		this.isDisabled = true;
		console.log(this.form.value);
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
			photoId: formValue.photo || undefined,
		};

		this.officeService.addOffice(office);
		const currentUrl = this.router.url;
		setTimeout(() => {
			this.isDisabled = false;
			this.dialogRef.close();
			this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
				this.router.navigate([currentUrl]);
			});
		}, 500);
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
}