import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Office} from "../interfaces/office.interface";
import {AppConfigService} from "./app-config.service";

@Injectable({
	providedIn: 'root'
})
export class OfficeService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: AppConfigService) {
		this.baseApiUrl = this.configService.officesUrl + '/api/';
	}

	getOffices() {
		return this.http.get<Office[]>(`${this.baseApiUrl}Offices`);
	}
}