import {inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {OidcSecurityService} from "angular-auth-oidc-client";
import {catchError, Observable, of} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
	providedIn: 'root'
})
export class FileService {
	http = inject(HttpClient);
	baseApiUrl: string;
	officeNoPhoto: string = "/assets/imgs/office-no-photo.png";

	constructor(private configService: AppConfigService, private oidc: OidcSecurityService) {
		this.baseApiUrl = this.configService.filesUrl + '/api';
	}

	getPhoto(photoId: string | undefined): Observable<string> {
		return this.http.get(`${this.baseApiUrl}/Photos/${photoId}`, {
			observe: 'response',
			responseType: 'text'
		}).pipe(
				map(response => {
					if (response.status === 200) {
						return response.body ?? this.officeNoPhoto;
					}
					else {
						return this.officeNoPhoto;
					}
				}),
				catchError(error => {
					console.error(`Failed to fetch photo:`, error);
					return of(this.officeNoPhoto);
				})
		);
	}
}