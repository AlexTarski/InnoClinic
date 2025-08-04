import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Patient} from "../interfaces/patient.interface";

@Injectable({
    providedIn: 'root'
})
export class PatientService {
    http = inject(HttpClient);
    baseApiUrl = 'http://localhost:5006/api/'

    getPatients() {
        return this.http.get<Patient[]>(`${this.baseApiUrl}Patients`)
    }
}