import {Component, inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import {OfficeService} from "../../data/services/office.service";
import {Office} from "../../data/interfaces/office.interface";

@Component({
  selector: 'app-offices',
  standalone: true,
	imports: [CommonModule],
  templateUrl: './offices.component.html',
  styleUrl: './offices.component.css'
})
export class OfficesComponent {
  officeService = inject(OfficeService);
  offices: Office[] = [];

  constructor(){
    this.officeService.getOffices()
        .subscribe(office => {
          this.offices = office
        });
  }

	createOffice(){}
}