import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Injectable({
	providedIn: 'root'
})
export class ReceptionistService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: AppConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.profilesUrl + '/api/Receptionists';
	}
}