import {Component, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";
import {DialogRef} from "@angular/cdk/dialog";

@Component({
  selector: 'app-create-office-form',
	imports: [
		FormsModule, ReactiveFormsModule, PhonePlusValidatorDirective,
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

	constructor(private dialogRef: DialogRef) {
	}

	onSubmit(){
		console.log(this.form.value);
	};

	close() {
		this.dialogRef.close();
	}
}
