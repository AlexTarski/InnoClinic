import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Doctor} from "../interfaces/doctor.interface";
import {AppConfigService} from "./app-config.service";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: AppConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.profilesUrl + '/api/Doctors';
	}

  getDoctors() {
  return this.http.get<Doctor[]>(`${this.baseApiUrl}`)
  }

	deactivateDoctorsByOfficeId(officeId: string | undefined) {
		this.oidc.getAccessToken().subscribe((token) => {
			const httpOptions = {
				headers: new HttpHeaders({
					Authorization: 'Bearer ' + token,
				}),
				responseType: 'text' as const,
			};

			this.http.put(`${this.baseApiUrl}/deactivate/byOfficeId/${officeId}`,
					{}, httpOptions)
					.subscribe({
						next: (res) => console.log('Doctors profiles deactivated!', res),
						error: (err) => console.error('Error deactivating doctors profiles:', err)
					});
		});
	}
}