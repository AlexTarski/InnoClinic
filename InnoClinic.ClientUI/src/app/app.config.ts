import {
	ApplicationConfig, inject, provideAppInitializer,
	provideBrowserGlobalErrorListeners,
	provideZoneChangeDetection
} from '@angular/core';
import {provideRouter} from '@angular/router';

import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptors,
} from "@angular/common/http";
import {
	authInterceptor,
	AuthInterceptor,
	LogLevel,
	provideAuth,
	StsConfigLoader,
	StsConfigStaticLoader
} from "angular-auth-oidc-client";
import {ConfigService} from "./data/services/config.service";

export function initConfig(configService: ConfigService) {
	return () => configService.load();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([])),
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
						redirectUrl: window.location.origin,
						postLogoutRedirectUri: window.location.origin,
						clientId: 'client_ui',
						scope: 'openid profile profiles email photo_id offline_access',
						responseType: 'code',
						silentRenew: true,
						useRefreshToken: true,
						autoUserInfo: true,
						logLevel: LogLevel.Debug,
					});
				}
			}
    }),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ]
};