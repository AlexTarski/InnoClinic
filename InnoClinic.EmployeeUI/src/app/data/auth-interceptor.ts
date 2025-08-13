import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import {AppConfigService} from "./services/app-config.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	authUrl: string;

	constructor(private router: Router,
							private appConfig: AppConfigService,) {
		this.authUrl = this.appConfig.authUrl;
	}

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		return next.handle(req).pipe(
				catchError((err: HttpErrorResponse) => {
					if (err.status === 401) {
						this.router.navigateByUrl('/login');
					} else if (err.status === 403) {
						this.router.navigateByUrl('/error/forbidden');
					} else if (err.status === 0 || err.status === 502 || err.status === 504) {
						if ((err.url || '').includes(this.authUrl)) {
							this.router.navigateByUrl('/error');
						}
					}
					return throwError(() => err);
				})
		);
	}
}