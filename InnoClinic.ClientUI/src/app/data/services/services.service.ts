import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
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
		return this.http.get<Service[]>(`${this.baseApiUrl}Services`);
	}

	getSpecializations() {
		return this.http.get<Specialization[]>(`${this.baseApiUrl}Specializations`);
	}

	getServiceCategories() {
		return this.http.get<ServiceCategory[]>(`${this.baseApiUrl}ServiceCategories`);
	}
}
