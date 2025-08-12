import {Component, OnInit, signal} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
	selector: 'app-root',
	imports: [RouterOutlet],
	template: `
			<router-outlet></router-outlet>
	`,
	styles: [`
	`],
})
export class App implements OnInit {
	protected readonly title = signal('InnoClinic');

	constructor(private oidc: OidcSecurityService) {}
	ngOnInit(): void {
		this.oidc.checkAuth().subscribe(({ isAuthenticated }) => {
			console.log('Authenticated:', isAuthenticated);
			if (!isAuthenticated) {
				this.oidc.authorize();
			}
		});
	}
}