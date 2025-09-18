import {Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatDialog} from "@angular/material/dialog";
import {OfficeService} from "../../data/services/office.service";
import {Router} from "@angular/router";
import {Office} from "../../data/interfaces/office.interface";
import {ConfirmDialog} from "../confirm-dialog/confirm-dialog";
import {PhonePlusValidatorDirective} from "../../data/directives/phone-plus-validator-directive";

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
		city: new FormControl("", [Validators.required]),
		street: new FormControl("", [Validators.required]),
		houseNumber: new FormControl("", [Validators.required]),
		officeNumber: new FormControl("", [Validators.required]),
		status: new FormControl(true, [Validators.required]),
		registryPhoneNumber: new FormControl("+", [Validators.required]),
		photo: new FormControl("")
	});

	constructor(private dialog: MatDialog,
							private officeService: OfficeService,
							private router: Router) {
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

	onSubmit(){
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
			photoId: formValue.photo || undefined,
		};

		this.officeService.updateOffice(office, this.office.id);
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
}
