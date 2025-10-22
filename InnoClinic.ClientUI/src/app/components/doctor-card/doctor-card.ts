import {Component, Input, ViewEncapsulation} from '@angular/core';
import {Doctor} from "../../data/interfaces/doctors.interface";

@Component({
  selector: 'app-doctor-card',
  imports: [
  ],
  templateUrl: `./doctor-card.component.html`,
  styleUrl: `./doctor-card.component.css`,
	encapsulation: ViewEncapsulation.Emulated
})
export class DoctorCard {
  @Input() doctor?: Doctor;
}