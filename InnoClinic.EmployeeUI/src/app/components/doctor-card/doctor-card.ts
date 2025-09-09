import {Component, Input} from '@angular/core';
import {Doctor} from "../../data/interfaces/doctor.interface";

@Component({
  selector: 'app-doctor-card',
  imports: [],
  templateUrl: `./doctor-card.component.html`,
  styleUrl: `./doctor-card.component.css`
})
export class DoctorCard {
  @Input() doctor?: Doctor;
}
