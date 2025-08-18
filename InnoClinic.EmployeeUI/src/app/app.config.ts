import {ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './app.routes';
import {provideHttpClient} from "@angular/common/http";
import {LogLevel, provideAuth} from 'angular-auth-oidc-client';

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZoneChangeDetection({eventCoalescing: true}),
		provideRouter(routes),
		provideHttpClient(),
		provideAuth({
			config: {
				authority: 'https://localhost:10036',
				redirectUrl: 'https://localhost:4300/login-success',
				postLogoutRedirectUri: window.location.origin,
				clientId: 'employee_ui',
				scope: 'openid profile profiles employee_ui offline_access email',
				responseType: 'code',
				silentRenew: true,
				useRefreshToken: true,
				logLevel: LogLevel.Debug,
			},
		}),
	]
};