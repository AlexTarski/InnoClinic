import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class IdpHealthService {
	private cached?: { ok: boolean; at: number };
	async check(): Promise<boolean> {
		const now = Date.now();
		if (this.cached && now - this.cached.at < 30000) return this.cached.ok;

		try {
			const ctrl = new AbortController();
			const t = setTimeout(() => ctrl.abort(), 3000);
			const res = await fetch('https://localhost:10036/.well-known/openid-configuration', { signal: ctrl.signal });
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