import {inject} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';
import {OidcSecurityService, PublicEventsService} from 'angular-auth-oidc-client';
import {firstValueFrom} from 'rxjs';
import {IdpHealthService} from "./services/idp-health.service";

export const canActivateAuth: CanActivateFn = async () => {
	const router = inject(Router);
	const idpService = inject(IdpHealthService);
	const oidc = inject(OidcSecurityService);

	const idpOk = await idpService.check();
	if (!idpOk) {
		return router.createUrlTree(['/error']);
	}

	const { isAuthenticated } = await firstValueFrom(oidc.checkAuth());
	if (isAuthenticated) {
		return true
	}

	return router.createUrlTree(['/login']);
}