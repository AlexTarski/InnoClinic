import {Component, Inject, Input} from '@angular/core';
import {DIALOG_DATA, DialogRef} from "@angular/cdk/dialog";
import {Office} from "../../data/interfaces/office.interface";
import {NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-office-card',
	imports: [
		NgOptimizedImage
	],
	template: `
		<div class="office-card">
			<div class="office-photo">
				<img ngSrc="/assets/imgs/office-no-photo.png" alt="SUCCESS!" width="350" height="350">
			</div>
			<div class="office-main-content">
				<div class="office-info-container">
					<div class="office-info">
						<h3>Office address:</h3>
						<h4>
							{{ office.office.address.city }},
							{{ office.office.address.street }},
							{{ office.office.address.houseNumber }},
							office {{ office.office.address.officeNumber }}
						</h4>
					</div>
					<div class="office-info">
						<h3>Registry phone number:</h3>
						<h4>{{ office.office.registryPhoneNumber }}</h4>
					</div>
					<div class="radioButtons">
						<label>
							<input type="radio" value=true [checked]="office.office.isActive" name="{{ office.office.id }}" disabled>
							Active
						</label>
						<label>
							<input type="radio" value=false [checked]="!office.office.isActive" name="{{ office.office.id }}" disabled>
							Inactive
						</label>
					</div>
				</div>
				<button class="editOffice-btn">Edit</button>
			</div>
			<button class="close-btn" (click)="close()">×</button>
		</div>
	`,
	styleUrl: `./office-card.component.css`
})
export class OfficeCard {
	constructor(
			@Inject(DIALOG_DATA) public office:
			{
				office: Office
			},
			private dialogRef: DialogRef
	) {}

	close() {
		this.dialogRef.close();
	}
}