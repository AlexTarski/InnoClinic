import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Doctor} from "../interfaces/doctors.interface";
import {ConfigService} from "./config.service";
@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService) {
		this.baseApiUrl = this.configService.get().Profiles_API_Url + '/api/';
	}


	getDoctors() {
		return this.http.get<Doctor[]>(`${this.baseApiUrl}Doctors`)
  }
}
