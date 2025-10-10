import {
	ApplicationConfig,
	inject,
	provideAppInitializer,
	provideBrowserGlobalErrorListeners,
	provideZoneChangeDetection
} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './app.routes';
import {provideHttpClient} from "@angular/common/http";
import {LogLevel, provideAuth, StsConfigLoader, StsConfigStaticLoader} from 'angular-auth-oidc-client';
import {ConfigService} from "./data/services/config.service";

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZoneChangeDetection({eventCoalescing: true}),
		provideRouter(routes),
		provideHttpClient(),
		provideAppInitializer(() => {
			const configService = inject(ConfigService);
			return configService.load(); // must return Promise<void>
		}),
		provideAuth({
			loader: {
				provide: StsConfigLoader,
				useFactory: () => {
					const configService = inject(ConfigService);
					const cfg = configService.get();
					return new StsConfigStaticLoader({
						authority: cfg.Auth_API_Url,
						redirectUrl: `${cfg.Employee_UI_Url}/login-success`,
						postLoginRoute: '/login-success',
						postLogoutRedirectUri: window.location.origin,
						clientId: 'employee_ui',
						scope: 'openid profile profiles offices employee_ui offline_access email',
						responseType: 'code',
						silentRenew: true,
						useRefreshToken: true,
						logLevel: LogLevel.Debug,
					})
				}
			}
		}),
	]
};