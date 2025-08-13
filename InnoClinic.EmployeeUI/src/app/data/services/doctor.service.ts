import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Doctor} from "../interfaces/doctor.interface";
import {AppConfigService} from "./app-config.service";

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: AppConfigService) {
		this.baseApiUrl = this.configService.profilesUrl + '/api/';
	}

  getDoctors() {
  return this.http.get<Doctor[]>(`${this.baseApiUrl}Doctors`)
  }
}