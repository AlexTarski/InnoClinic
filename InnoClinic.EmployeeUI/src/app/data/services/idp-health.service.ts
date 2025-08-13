import { Injectable } from '@angular/core';
import {AppConfigService} from "./app-config.service";

@Injectable({ providedIn: 'root' })
export class IdpHealthService {
	private cached?: { ok: boolean; at: number };
	authUrl: string;
	constructor(private configService: AppConfigService) {
		this.authUrl = this.configService.authUrl;
	}

	async check(): Promise<boolean> {
		const now = Date.now();
		if (this.cached && now - this.cached.at < 30000) return this.cached.ok;

		try {
			const ctrl = new AbortController();
			const t = setTimeout(() => ctrl.abort(), 3000);
			const res = await fetch(`${this.authUrl}/.well-known/openid-configuration`, { signal: ctrl.signal });
			clearTimeout(t);
			const ok = res.ok;
			this.cached = { ok, at: now };
			return ok;
		} catch {
			this.cached = { ok: false, at: now };
			return false;
		}
	}
}