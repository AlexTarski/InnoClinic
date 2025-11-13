import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Doctor} from "../interfaces/doctor.interface";
import {ConfigService} from "./config.service";
import {Observable} from "rxjs";
import {Receptionist} from "../interfaces/receptionist.interface";
import {UserRole} from "../enums/userRole";
@Injectable({
	providedIn: 'root'
})
export class UserService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService) {
		this.baseApiUrl = this.configService.get().Profiles_API_Url + '/api';
	}

	getUserProfile(role: string, accountId: string): Observable<any> {
		switch (role) {
			case UserRole.Doctor:
				return this.http.get<Doctor>(`${this.baseApiUrl}/Doctors/accountId/${accountId}`);
			case UserRole.Receptionist:
				return this.http.get<Receptionist>(`${this.baseApiUrl}/Receptionists/accountId/${accountId}`);
			default:
				throw new Error("Unknown user role");
		}
	}
}