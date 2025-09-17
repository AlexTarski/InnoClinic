import {Component, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";
import {Dialog, DialogRef} from "@angular/cdk/dialog";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDialog} from "../confirm-dialog/confirm-dialog";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-create-office-form',
	imports: [
		FormsModule, ReactiveFormsModule, PhonePlusValidatorDirective, NgClass,
	],
  templateUrl: `./create-office-form.html`,
  styleUrl: `./create-office-form.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class CreateOfficeForm {
	form: FormGroup = new FormGroup({
		city: new FormControl("", [Validators.required]),
		street: new FormControl("", [Validators.required]),
		houseNumber: new FormControl("", [Validators.required]),
		officeNumber: new FormControl("", [Validators.required]),
		status: new FormControl(true, [Validators.required]),
		registryPhoneNumber: new FormControl("+", [Validators.required]),
		photo: new FormControl("")
	});

	constructor(private dialog: Dialog, private dialogRef: DialogRef) {
	}

	onSubmit(){
		console.log(this.form.value);
	};

	onCancel() {
		const ref = this.dialog.open(ConfirmDialog, {
		disableClose: true });

		ref.closed.subscribe(result => {
		if (result) {
			this.dialogRef.close()
		}
		});
	}
}