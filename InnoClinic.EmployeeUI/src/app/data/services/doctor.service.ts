import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Doctor} from "../interfaces/doctor.interface";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {ConfigService} from "./config.service";
import {switchMap} from "rxjs";
import {ProfilesApiQueryParams} from "../interfaces/profilesApiQueryParams.interface";
import {DatePipe} from "@angular/common";

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.get().Profiles_API_Url + '/api/Doctors';
	}

	getDoctors() {
		return this.oidc.getAccessToken().pipe(
				switchMap(token => {
					const queryParams: ProfilesApiQueryParams = new ProfilesApiQueryParams(false);
					const httpOptions = {
						headers: new HttpHeaders({Authorization: 'Bearer ' + token}),
						params: queryParams.toHttpParams()
					};

					return this.http.get<Doctor[]>(`${this.baseApiUrl}`, httpOptions)
				})
		);
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