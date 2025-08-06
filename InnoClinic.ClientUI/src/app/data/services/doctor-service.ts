import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Doctor} from "../interfaces/doctors.interface";

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
  baseApiUrl = 'https://localhost:7036/api/'

  getDoctors() {
  return this.http.get<Doctor[]>(`${this.baseApiUrl}Doctors`)
  }
}
