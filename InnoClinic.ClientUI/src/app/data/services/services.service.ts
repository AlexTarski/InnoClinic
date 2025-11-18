import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {ConfigService} from "./config.service";
import {Service} from "../interfaces/services/service.interface";
import {Specialization} from "../interfaces/services/specialization.interface";
import {ServiceCategory} from "../interfaces/services/serviceCategory.interface";
@Injectable({
	providedIn: 'root'
})
export class ServicesService {
	http = inject(HttpClient);
	baseApiUrl: string;

	constructor(private configService: ConfigService) {
		this.baseApiUrl = this.configService.get().Services_API_Url + '/api/';
	}


	getServices() {
		let params = new HttpParams()
				.set("PageNumber", 1)
				.set("PageSize", 50);

		return this.http.get<Service[]>(`${this.baseApiUrl}Services`, {params});
	}

	getSpecializations() {
		return this.http.get<Specialization[]>(`${this.baseApiUrl}Specializations`);
	}

	getServiceCategories() {
		return this.http.get<ServiceCategory[]>(`${this.baseApiUrl}ServiceCategories`);
	}
}
