import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Office} from "../interfaces/office.interface";
import {ConfigService} from "./config.service";

@Injectable({
	providedIn: 'root'
})
export class OfficeService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService) {
		this.baseApiUrl = this.configService.get().Offices_API_Url + '/api/';
	}

	getOffice(officeId: string) {
		return this.http.get<Office>(`${this.baseApiUrl}Offices/${officeId}`);
	}
}