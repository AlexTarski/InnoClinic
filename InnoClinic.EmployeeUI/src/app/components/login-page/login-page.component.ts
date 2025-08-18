import {Component, Inject, inject, OnInit} from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import {Router} from "@angular/router";
import {AppConfigService} from "../../data/services/app-config.service";

@Component({
	template: `
    <h1>Login</h1>
  `
})
export class LoginComponent implements OnInit {
	oidc = inject(OidcSecurityService);
	baseUrl?: string;

	constructor(private configService: AppConfigService) {
		this.baseUrl = configService.employeeUiUrl;
	}

	ngOnInit() {
		const authOptions = {
			redirectUrl: `${this.baseUrl}/login-success`,
		};

		this.oidc.authorize(undefined, authOptions)
	}
}