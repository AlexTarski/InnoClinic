import {HttpParams} from "@angular/common/http";

export class ProfilesApiQueryParams {
	onlyActiveProfiles?: boolean;

	constructor(onlyActiveProfiles: boolean) {
		this.onlyActiveProfiles = onlyActiveProfiles;
	}

	toHttpParams(): HttpParams {
		return new HttpParams().set('onlyActiveProfiles', String(this.onlyActiveProfiles));
	}
}