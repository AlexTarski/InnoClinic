import {inject, Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {catchError, firstValueFrom, Observable, of, throwError} from "rxjs";
import {map} from "rxjs/operators";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {ConfigService} from "./config.service";

@Injectable({
	providedIn: 'root'
})
export class FileService {
	http = inject(HttpClient);
	baseApiUrl: string;
	authApiUrl: string;
	officeNoPhoto: string = "/assets/imgs/office-no-photo.png";
	clientNoPhoto: string = "/assets/imgs/client-no-photo.png";
	employeeNoPhoto: string = "/assets/imgs/employee-no-photo.png";

	constructor(private configService: ConfigService,
							private sanitizer: DomSanitizer) {
		this.baseApiUrl = this.configService.get().Docs_API_Url + '/api';
		this.authApiUrl = this.configService.get().Auth_API_Url + '/api';
	}

  async getDoctorPhoto(accountId: string): Promise<SafeUrl> {
		try {
			const photoId = await firstValueFrom(this.getPhotoId(accountId));
			const response = await firstValueFrom(this.getPhoto(photoId));

			if (response.status === 200) {
				return this.sanitizer.bypassSecurityTrustResourceUrl(JSON.parse(response.body!));
			}
			else {
				console.error(response.status);
				return this.sanitizer.bypassSecurityTrustResourceUrl(this.employeeNoPhoto);
			}

		}
		catch (error: any) {
			if (error.status === 404) {
				console.warn("Account with this ID not found");
				return this.sanitizer.bypassSecurityTrustResourceUrl(this.employeeNoPhoto);
			}
			else {
				console.error("Unexpected error: " + error);
				return this.sanitizer.bypassSecurityTrustResourceUrl(this.employeeNoPhoto);
			}
		}
	}

	getOfficePhoto(photoId: string | undefined): Observable<SafeUrl> {
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

	private getPhoto(photoId: string): Observable<HttpResponse<string>> {
		return this.http.get(`${this.baseApiUrl}/Photos/${photoId}`, {
			observe: 'response',
			responseType: 'text'
		});
	}

	private getPhotoId(accountId: string): Observable<string> {
		return this.http.get<string>(`${this.authApiUrl}/AccountData/photoId/${accountId}`, {
			observe: 'response',
			responseType: 'json'
		}).pipe(
				map((response) => {
					return response.body as string;
				}),
				catchError(err => {
					if (err.status === 404) {
						return throwError(() => ({ status: 404, message: 'Account not found' }));
					}
					return throwError(() => err);
				})
		);
	}
}