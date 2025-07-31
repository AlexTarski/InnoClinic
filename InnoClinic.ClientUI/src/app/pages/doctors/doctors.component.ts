import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {DoctorCard} from "../../components/doctor-card/doctor-card";

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [CommonModule, DoctorCard],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css'
})
export class DoctorsComponent {}