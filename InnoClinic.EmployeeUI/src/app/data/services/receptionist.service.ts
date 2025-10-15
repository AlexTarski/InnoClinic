import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {ConfigService} from "./config.service";

@Injectable({
	providedIn: 'root'
})
export class ReceptionistService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.get().Profiles_API_Url + '/api/Receptionists';
	}
}