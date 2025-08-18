import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { firstValueFrom } from 'rxjs';
import {IdpHealthService} from "./services/idp-health.service";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
	constructor(
			private oidc: OidcSecurityService,
			private router: Router,
			private idp: IdpHealthService
	) {}

	async canActivate(): Promise<boolean | UrlTree> {
		const idpOk = await this.idp.check();
		if (!idpOk) {
			return this.router.parseUrl('/error');
		}

		const isAuth = await firstValueFrom(this.oidc.isAuthenticated$);
		if (isAuth) return true;

		return this.router.parseUrl('/login');
	}
}