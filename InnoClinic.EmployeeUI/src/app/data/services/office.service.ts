import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Office} from "../interfaces/office.interface";
import {AppConfigService} from "./app-config.service";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Injectable({
	providedIn: 'root'
})
export class OfficeService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: AppConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.officesUrl + '/api/';
	}

	getOffices() {
		return this.http.get<Office[]>(`${this.baseApiUrl}Offices`);
	}

	addOffice(office: Office) {
		this.oidc.getAccessToken().subscribe((token) => {
			const httpOptions = {
				headers: new HttpHeaders({
					Authorization: 'Bearer ' + token,
				}),
				responseType: 'text' as const,
			};


			this.http.post(`${this.baseApiUrl}Offices`, office, httpOptions)
					.subscribe({
						next: (res) => console.log('Office added:', res),
						error: (err) => console.error('Error adding office:', err)
			});
		});
	}
}