import {Component, Input} from '@angular/core';
import {Patient} from "../../data/interfaces/patient.interface";

@Component({
  selector: 'app-patient-card',
  imports: [
  ],
  templateUrl: `./patient-card.component.html`,
  styleUrl: `./patient-card.component.css`
})
export class PatientCard {
  @Input() patient?: Patient;
}
