import {Component, Inject, ViewEncapsulation} from '@angular/core';
import {DIALOG_DATA, DialogRef} from "@angular/cdk/dialog";
import {Office} from "../../data/interfaces/office.interface";
import {NgOptimizedImage} from "@angular/common";
import {EditOfficeForm} from "../edit-office-form/edit-office-form";
import {Router} from "@angular/router";

@Component({
  selector: 'app-office-card',
	imports: [
		NgOptimizedImage,
		EditOfficeForm
	],
	template: `
		<div class="component-container">
			<div class="office-photo">
				<img ngSrc="/assets/imgs/office-no-photo.png" alt="office-photo" width="300" height="300">
			</div>
			@if(isEditing)
			{
				<div class="office-main-content">
					<app-edit-office-form [office]="office.office" (onChanged)="onChanged($event)"></app-edit-office-form>
				</div>
			} 
			@else {
				<div class="office-main-content">
					<div class="office-info-container">
						<div class="office-info">
							<h3 class="component-header">Office address:</h3>
							<h4 class="component-text">
								{{ office.office.address.city }},
								{{ office.office.address.street }},
								{{ office.office.address.houseNumber }},
								office {{ office.office.address.officeNumber }}
							</h4>
						</div>
						<div class="office-info">
							<h3 class="component-header">Registry phone number:</h3>
							<h4 class="component-text">{{ office.office.registryPhoneNumber }}</h4>
						</div>
						<div class="office-status">
							@if (office.office.isActive) {
								<div class="status">✅ Active</div>
							} @else {
								<div class="status">❌ Inactive</div>
							}
						</div>
					</div>
					<button class="main-positive-btn" (click)="editOffice()">Edit</button>
				</div>
				<button class="close-btn" (click)="close()">×</button>
			}
		</div>
	`,
	styleUrl: `./office-card.component.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class OfficeCard {
	public isEditing: boolean = false
	public wasEdited: boolean = false

	constructor(
			@Inject(DIALOG_DATA) public office:
			{
				office: Office
			},
			private dialogRef: DialogRef<OfficeCard>,
			private router: Router,
	)
	{
		this.dialogRef.closed.subscribe(() =>
		{
			if(this.wasEdited)
			{
				const currentUrl = this.router.url;
				this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
					this.router.navigate([currentUrl]);
				});
			}
		});
	}

	onChanged(data:{ edited: boolean; office: Office | undefined }) {
		if (data.edited) {
			this.office.office = data.office!
			this.wasEdited = true;
		}

		this.isEditing = false;
		this.dialogRef.disableClose = false;
	}

	editOffice() {
		this.isEditing=true
		this.dialogRef.disableClose = true;
	}

	close() {
		this.dialogRef.close();
	}
}