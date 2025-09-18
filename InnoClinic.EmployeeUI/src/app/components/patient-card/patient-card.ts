import {Component, Input, ViewEncapsulation} from '@angular/core';
import {Patient} from "../../data/interfaces/patient.interface";

@Component({
  selector: 'app-patient-card',
  imports: [
  ],
  templateUrl: `./patient-card.component.html`,
  styleUrl: `./patient-card.component.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class PatientCard {
  @Input() patient?: Patient;
}
