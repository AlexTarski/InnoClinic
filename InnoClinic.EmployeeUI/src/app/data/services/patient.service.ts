import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Patient} from "../interfaces/patient.interface";
import {AppConfigService} from "./app-config.service";

@Injectable({
    providedIn: 'root'
})
export class PatientService {
    http = inject(HttpClient);
		baseApiUrl: string;

	constructor(private configService: AppConfigService) {
		this.baseApiUrl = this.configService.profilesUrl + '/api/';
	}

    getPatients() {
        return this.http.get<Patient[]>(`${this.baseApiUrl}Patients`)
    }
}