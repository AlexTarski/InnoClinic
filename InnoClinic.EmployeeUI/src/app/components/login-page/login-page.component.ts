import {Component, inject, OnInit} from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	template: `
    <h1>Login</h1>
  `
})
export class LoginComponent implements OnInit {
	oidc = inject(OidcSecurityService);

	ngOnInit() {
		this.oidc.authorize();
	}
}