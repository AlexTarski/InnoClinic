import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Doctor} from "../interfaces/doctor.interface";

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
  baseApiUrl = 'http://localhost:5006/api/'

  getDoctors() {
  return this.http.get<Doctor[]>(`${this.baseApiUrl}Doctors`)
  }
}