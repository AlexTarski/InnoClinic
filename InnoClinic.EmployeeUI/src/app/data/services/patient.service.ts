import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Patient} from "../interfaces/patient.interface";
import {ConfigService} from "./config.service";

@Injectable({
    providedIn: 'root'
})
export class PatientService {
    http = inject(HttpClient);
		baseApiUrl: string;

	constructor(private configService: ConfigService) {
		this.baseApiUrl = this.configService.get().Profiles_API_Url + '/api/';
	}

    getPatients() {
        return this.http.get<Patient[]>(`${this.baseApiUrl}Patients`)
    }
}