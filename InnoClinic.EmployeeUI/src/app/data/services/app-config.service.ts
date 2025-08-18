import {Injectable} from "@angular/core";

@Injectable({
	providedIn: 'root'
})
export class AppConfigService {
	private config = window['appConfig'];

	get authUrl(): string {
		return this.config?.authUrl;
	}

	get profilesUrl(): string {
		return this.config?.profilesUrl;
	}

	get employeeUiUrl(): string {
		return this.config?.employeeUiUrl;
	}
}