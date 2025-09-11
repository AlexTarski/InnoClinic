import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
	constructor(private oidcSecurityService: OidcSecurityService,
							private router: Router) {}

	canActivate(route: ActivatedRouteSnapshot) {
		const allowedRoles = route.data['roles'] as string[];

		return this.oidcSecurityService.getPayloadFromAccessToken().pipe(
				map(payload => {
					const userRoles: string[] = Array.isArray(payload.role)
							? payload.role
							: [payload.role];

					const hasRole = allowedRoles.some(r => userRoles.includes(r));

					if (!hasRole) {
						this.router.navigate(['/forbidden']);
						return false;
					}
					return true;
				})
		);
	}
}