import {Component, inject, OnInit} from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	template: `
    redirecting to login page...
  `
})
export class LoginComponent implements OnInit {
	oidc = inject(OidcSecurityService);

	ngOnInit() {
				this.oidc.authorize(undefined, {
					customParams: {
						prompt: 'login'
					}
				});
	}
}