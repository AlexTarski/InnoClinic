import {Component, inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import {PatientCard} from "../../components/patient-card/patient-card";
import {Patient} from "../../data/interfaces/patient.interface";
import {PatientService} from "../../data/services/patient.service";

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [CommonModule, PatientCard],
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.css'
})
export class PatientsComponent {
  patientService = inject(PatientService);
  patients: Patient[] = [];

  constructor(){
    this.patientService.getPatients()
        .subscribe(patient => {
          this.patients = patient
        });
  }
}