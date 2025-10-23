import {inject, Injectable} from "@angular/core";
import {HttpClient, HttpResponse} from "@angular/common/http";
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
	clientNoPhoto: string = "/assets/imgs/client-no-photo.png";
	employeeNoPhoto: string = "/assets/imgs/employee-no-photo.png";

	constructor(private configService: ConfigService,
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