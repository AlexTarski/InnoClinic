import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export interface AppConfig {
	Profiles_API_Url: string;
	Offices_API_Url: string;
	Auth_API_Url: string;
	Docs_API_Url: string;
}

@Injectable({ providedIn: 'root' })
export class ConfigService {
	private config!: AppConfig;

	constructor(private http: HttpClient) {}

	async load(): Promise<void> {
		const cfg = await firstValueFrom(
				this.http.get<AppConfig>('/assets/config/config.json')
		);
		this.config = cfg;
	}

	get(): AppConfig {
		if (!this.config) throw new Error('Config not loaded yet');
		return this.config;
	}
}
