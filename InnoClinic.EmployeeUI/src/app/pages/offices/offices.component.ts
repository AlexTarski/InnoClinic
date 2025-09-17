import {Component, inject, ViewEncapsulation} from '@angular/core';
import { CommonModule } from '@angular/common';
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";
import {OfficeCard} from "../../components/office-card/office-card";
import {FormsModule} from "@angular/forms";
import {CreateOfficeForm} from "../../components/create-office-form/create-office-form";
import {Dialog} from "@angular/cdk/dialog";

@Component({
  selector: 'app-offices',
  standalone: true,
	imports: [CommonModule, FormsModule],
  templateUrl: './offices.component.html',
  styleUrl: './offices.component.css',
	encapsulation: ViewEncapsulation.Emulated
})
export class OfficesComponent {
  private officeService = inject(OfficeService);
	private dialog = inject(Dialog)
  offices: Office[] = [];

  constructor() {
		this.officeService.getOffices()
				.subscribe(offices => {
					this.offices = offices
				});
  }

	createOffice(){
		this.dialog.open(CreateOfficeForm, {
			disableClose: true
		});
	}

	viewOfficeInfo(office: Office){
		this.dialog.open(OfficeCard,
				{
					data: {office: office},
				});
	}
}