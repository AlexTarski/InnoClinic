import {Component, inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import {DoctorCard} from "../../components/doctor-card/doctor-card";
import {DoctorService} from "../../data/services/doctor.service";
import {Doctor} from "../../data/interfaces/doctor.interface";

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [CommonModule, DoctorCard],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css'
})
export class DoctorsComponent {
  doctorService = inject(DoctorService);
  doctors: Doctor[] = [];

  constructor(){
    this.doctorService.getDoctors()
        .subscribe(doctor => {
          this.doctors = doctor
        });
  }
}