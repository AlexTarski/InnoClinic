import {inject, Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {catchError, Observable, of} from "rxjs";
import {map} from "rxjs/operators";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {ConfigService} from "./config.service";

@Injectable({
	providedIn: 'root'
})
export class FileService {
	http = inject(HttpClient);
	baseApiUrl: string;
	officeNoPhoto: string = "/assets/imgs/office-no-photo.png";

	constructor(private configService: ConfigService,
							private oidc: OidcSecurityService,
							private sanitizer: DomSanitizer) {
		this.baseApiUrl = this.configService.get().Docs_API_Url + '/api';
	}

	getPhoto(photoId: string | undefined): Observable<SafeUrl> {
		return this.http.get(`${this.baseApiUrl}/Photos/${photoId}`, {
			observe: 'response',
			responseType: 'text'
		}).pipe(
				map(response => {
					if (response.status === 200) {
						return this.sanitizer.bypassSecurityTrustResourceUrl(JSON.parse(response.body!));
					}
					else {
						return this.sanitizer.bypassSecurityTrustResourceUrl(this.officeNoPhoto);
					}
				}),
				catchError(error => {
					console.warn(`Failed to fetch photo:`, error);
					return of(this.sanitizer.bypassSecurityTrustResourceUrl(this.officeNoPhoto));
				})
		);
	}

	addOfficePhoto(photoFile: File) {
		const formData = new FormData();
		formData.append('formFile', photoFile, photoFile.name);
		return this.http.post(`${this.baseApiUrl}/Photos/Offices`, formData, {
			observe: 'response',
			responseType: 'json'
		}).pipe(
				map(response => {
					if (response.status === 200) {
						console.info("Photo uploaded successfully", response.body);
						return <string>response.body;
					}
					else {
						console.error("Failed to upload photo", response.statusText);
						throw new HttpErrorResponse({});
					}
				})
		)
	}

	updatePhoto(photoId: string | undefined, photoFile: File) {
		const formData = new FormData();
		formData.append('formFile', photoFile, photoFile.name);
		return this.http.put(`${this.baseApiUrl}/Photos/${photoId}`, formData, {
			observe: 'response',
			responseType: 'json'
		}).pipe(
				catchError(err => {
					if (err.status === 404) {
						return of({ status: 404 } as HttpResponse<any>);
					}
					throw err;
				})
		);
	}
}