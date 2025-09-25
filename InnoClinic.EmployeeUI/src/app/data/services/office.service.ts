import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Office} from "../interfaces/office.interface";
import {AppConfigService} from "./app-config.service";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {switchMap} from "rxjs";

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

	getOffice(officeId: string) {
		return this.http.get<Office>(`${this.baseApiUrl}Offices/${officeId}`);
	}

	addOffice(office: Office) {
		return this.oidc.getAccessToken().pipe(
				switchMap(token => {
					const httpOptions = {
						headers: new HttpHeaders({ Authorization: 'Bearer ' + token }),
						responseType: 'text' as const,
					};
					return this.http.post(`${this.baseApiUrl}Offices`, office, httpOptions);
				})
		);
	}

	updateOffice(office: Office, officeId: string | undefined) {
		return this.oidc.getAccessToken().pipe(
				switchMap(token => {
					const httpOptions = {
						headers: new HttpHeaders({ Authorization: 'Bearer ' + token }),
						responseType: 'text' as const,
					};
					return this.http.put(`${this.baseApiUrl}Offices/${officeId}`, office, httpOptions);
				})
		);
	}
}